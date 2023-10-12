using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player _player;

    [Header("Movement")]
    [SerializeField] private GameObject _playerObj;
    public float moveSpeed;
    public float jumpForce;
    private Vector2 _curMovementInput;
    private float _originMoveSpeed;
    private bool _isCrouch = false;
    private float _plusRotateValue = 60f;

    [Header("Look")]
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    private CinemachinePOV _pov;

    [Header("Audoi")]
    [SerializeField] private AudioSource _jumpAudioSource;
    [SerializeField] private AudioSource _reloadAudioSource;

    private Coroutine _fireCoroutine;
    private bool _isFiring = false;
    private bool _canFiring = true;
    private bool _isReloading = false;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _pov = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        _originMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        Move();

        if (!_reloadAudioSource.isPlaying)
        {
            _isReloading = false;
            _canFiring = true;
        }
    }

    private void Move()
    {
        Vector3 dir = GetMovementDirection(_curMovementInput);
        _player.Controller.Move((dir * moveSpeed + _player.ForceReceiver.Movement) * Time.deltaTime);

    }

    private Vector3 GetMovementDirection(Vector2 movementInput)
    {
        float verticalAxis = _pov.m_VerticalAxis.Value;
        float horizontalAxis = _pov.m_HorizontalAxis.Value;
        Quaternion quanternion = Quaternion.Euler(verticalAxis, horizontalAxis, 0);

        _playerObj.transform.rotation = Quaternion.Euler(0, _pov.m_HorizontalAxis.Value + _plusRotateValue, 0);

        Vector3 forward = quanternion * _virtualCamera.LookAt.forward;
        Vector3 right = quanternion * _virtualCamera.LookAt.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * movementInput.y + right * movementInput.x;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _curMovementInput = context.ReadValue<Vector2>();
            _player.Animator.SetFloat("DirX", _curMovementInput.x);
            _player.Animator.SetFloat("DirZ", _curMovementInput.y);
            _player.Animator.SetBool("Run", true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _player.Animator.SetBool("Run", false);
            _curMovementInput = Vector2.zero;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            _isCrouch = !_isCrouch;
            if (_isCrouch)
            {
                _player.Animator.SetBool("Crouch", true);
                moveSpeed *= 0.5f;
            }
            else
            {
                _player.Animator.SetBool("Crouch", false);
                moveSpeed = _originMoveSpeed;
            }
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (_player.Controller.isGrounded)
            {
                if (!_isCrouch)
                    _jumpAudioSource.Play();
                _player.Animator.SetTrigger("Jump");
                _player.ForceReceiver.Jump(jumpForce);
            }
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            _player.Animator.SetBool("Aim", true);
            _isFiring = true;
            _fireCoroutine = StartCoroutine(FireCO());
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _player.Animator.SetBool("Aim", false);
            _isFiring = false;
            if (_fireCoroutine != null)
                StopCoroutine(_fireCoroutine);
        }
    }

    IEnumerator FireCO()
    {
        while (_canFiring && _isFiring)
        {
            _player.Weapon.Fire();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (!_isReloading)
            if (context.phase == InputActionPhase.Started)
            {
                _canFiring = false;
                _player.Weapon.ReloadBullet(_player.Weapon.ReloadBulletCount);
                _reloadAudioSource.Play();
                _isReloading = true;
            }
    }
}

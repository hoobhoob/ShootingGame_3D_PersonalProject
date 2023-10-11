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

    [Header("Look")]
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    private CinemachinePOV _pov;

    private Coroutine _fireCoroutine;
    private bool _IsFiring = false;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _pov = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    private void Update()
    {
        Move();
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

        _playerObj.transform.rotation = Quaternion.Euler(0, _pov.m_HorizontalAxis.Value + 45, 0);

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
            _player.Animator.SetBool("Run", true);
            _curMovementInput = context.ReadValue<Vector2>();

        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _player.Animator.SetBool("Run", false);
            _curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (_player.Controller.isGrounded)
            {
                _player.Animator.SetTrigger("Jump");
                _player.ForceReceiver.Jump(jumpForce);
            }
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            //_player.Weapon.Fire();
            _IsFiring = true;
            _fireCoroutine = StartCoroutine(FireCO());
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _IsFiring = false;
            StopCoroutine(_fireCoroutine);
        }
    }

    IEnumerator FireCO()
    {
        while (_IsFiring)
        {
            _player.Weapon.Fire();
            yield return new WaitForSeconds(0.1f);
        }
    }
}

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

    private Vector3 GetForwardDirection()
    {
        float verticalAxis = _pov.m_VerticalAxis.Value;
        float horizontalAxis = _pov.m_HorizontalAxis.Value;
        Quaternion quanternion = Quaternion.Euler(verticalAxis, horizontalAxis, 0);

        Vector3 right = quanternion * _virtualCamera.LookAt.right;
        right.y = 0;
        right.Normalize();

        return right;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _player.Animator.SetBool("@Run", true);
            _curMovementInput = context.ReadValue<Vector2>();

        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _player.Animator.SetBool("@Run", false);
            _curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("OnJump");
            if (_player.Controller.isGrounded)
            {
                _player.ForceReceiver.Jump(jumpForce);
                Debug.Log("isGrounded : OnJump");
            }
        }
    }

}

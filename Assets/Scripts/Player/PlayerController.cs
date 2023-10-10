using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player _player;

    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpForce;
    //public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public CinemachineVirtualCamera virtualCamera;
    public CinemachinePOV pov;
    public float minXlook;
    public float maxXlook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;


    private void Awake()
    {
        _player = GetComponent<Player>();
        pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    private void Move()
    {
        _player.Controller.Move(GetMovementDirection(curMovementInput) * moveSpeed * Time.deltaTime);
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXlook, maxXlook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    private Vector3 GetMovementDirection(Vector2 movementInput)
    {
        float verticalAxis = pov.m_VerticalAxis.Value;
        float horizontalAxis = pov.m_HorizontalAxis.Value;
        var quanternion = Quaternion.Euler(verticalAxis, horizontalAxis, 0);

        Vector3 forward = quanternion * virtualCamera.LookAt.forward;
        Vector3 right = quanternion * virtualCamera.LookAt.right;

        Debug.Log("현재 forward : " + forward);
        Debug.Log("현재 right : " + right);

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * movementInput.y + right * movementInput.x;
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();

        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (_player.Controller.isGrounded)
                _player.Controller.Move(Vector2.up * jumpForce * Time.deltaTime);
            //_rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }

    //private bool IsGrounded()
    //{
    //    Ray[] rays = new Ray[4]
    //    {
    //        new Ray(transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
    //        new Ray(transform.position + (-transform.forward * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
    //        new Ray(transform.position + (transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
    //        new Ray(transform.position + (-transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
    //    };

    //    for (int i = 0; i < rays.Length; i++)
    //    {
    //        if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (transform.right * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.right * 0.2f), Vector3.down);
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}

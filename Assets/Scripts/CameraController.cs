using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Z-axis is Forward and Back
    // X-axis is Left and Right
    // Y-Axis is Up and Down
    // We want to move the Camera forward and back based on WS inputs, and left and right based on AD inputs.

    [SerializeField] private CinemachineVirtualCamera _cinemacineVirtualCamera;
    private CinemachineTransposer _cineMachineTransposer;
    private Vector3 _transposerFollowOffset;

    private const float _MIN_FOLLOW_Y_OFFSET = 2f;
    private const float _MAX_FOLLOW_Y_OFFSET = 12f;

    private void Start()
    {
        _cineMachineTransposer = _cinemacineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _transposerFollowOffset = _cineMachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        HandleCameraMovement();
        HandleCameraRotation();
        HandleCameraZoom();
    }

    public void HandleCameraMovement()
    {
        Vector3 _inputMoveDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            _inputMoveDirection.z = +1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            _inputMoveDirection.z = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            _inputMoveDirection.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _inputMoveDirection.x = +1f;
        }

        float moveSpeed = 10f;
        Vector3 moveVector = _inputMoveDirection.z * transform.forward + _inputMoveDirection.x * transform.right;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    public void HandleCameraRotation()
    {
        Vector3 _inputRotationDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.Q))
        {
            _inputRotationDirection.y = +1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            _inputRotationDirection.y = -1f;
        }

        float rotationSpeed = 100f;
        Vector3 rotationVector = _inputRotationDirection * rotationSpeed * Time.deltaTime;
        transform.eulerAngles += rotationVector;
    }

    public void HandleCameraZoom()
    {
        float zoomAmount = 1f;

        if (Input.mouseScrollDelta.y < 0)
        {
            _transposerFollowOffset.y += zoomAmount;
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            _transposerFollowOffset.y -= zoomAmount;
        }

        _transposerFollowOffset.y = Mathf.Clamp(_transposerFollowOffset.y, _MIN_FOLLOW_Y_OFFSET, _MAX_FOLLOW_Y_OFFSET);

        float zoomSpeed = 5f;
        _cineMachineTransposer.m_FollowOffset = Vector3.Lerp(_cineMachineTransposer.m_FollowOffset, _transposerFollowOffset, Time.deltaTime * zoomSpeed);
    }
}

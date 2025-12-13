using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform FollowPoint;
    [SerializeField] private CinemachineCamera CinemachineCamera;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool moveInLocalSpace = false;
    [SerializeField] private float moveSmoothTime = 0.12f;

    [Header("Rotation")]
    [SerializeField] private bool isRotationAllowed = false;
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float rotationSmoothTime = 0.12f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 6f;
    [SerializeField] private float minZoomDistance = -2f;
    [SerializeField] private float maxZoomDistance = 20f;
    [SerializeField] private float zoomSmoothTime = 0.12f;
    [SerializeField] private float zoomKeySpeed = 4f;

    [Header("Cinemachine FOV")]
    [SerializeField] private float minFOV = 40f;
    [SerializeField] private float maxFOV = 70f;
    [SerializeField] private float fovSmoothTime = 0.1f;

    private Vector3 baseTargetPosition;
    private Vector3 moveVelocity = Vector3.zero;

    private float targetYaw;
    private float currentYaw;
    private float rotationVelocity = 0f;

    private float targetZoomDistance;
    private float currentZoomDistance;
    private float zoomVelocity = 0f;

    private float fovVelocity = 0f;
    private float currentFOV;
    private float targetFOV;

    void Start()
    {
        if (FollowPoint == null)
        {
            enabled = false;
            return;
        }

        baseTargetPosition = FollowPoint.position;
        currentYaw = targetYaw = FollowPoint.eulerAngles.y;
        currentZoomDistance = targetZoomDistance = 0f;

        if (CinemachineCamera != null)
        {
            currentFOV = targetFOV = CinemachineCamera.Lens.OrthographicSize;
        }
    }

    void Update()
    {
        if (FollowPoint == null) return;

        HandleMovementInput();
        HandleRotationInput();
        HandleZoomInput();
        ApplySmoothedTransform();
    }

    private void HandleMovementInput()
    {
        Vector2 input = Vector2.zero;

        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.wKey.isPressed) input.y += 1f;
            if (kb.sKey.isPressed) input.y -= 1f;
            if (kb.dKey.isPressed) input.x += 1f;
            if (kb.aKey.isPressed) input.x -= 1f;
        }

        var gp = Gamepad.current;
        if (gp != null)
        {
            input += gp.leftStick.ReadValue();
        }

        if (input.sqrMagnitude > 1f) input.Normalize();

        if (input != Vector2.zero)
        {
            Transform rotationSource = null;

            if (Camera.main != null)
                rotationSource = Camera.main.transform;
            else if (CinemachineCamera != null)
                rotationSource = CinemachineCamera.transform;

            Vector3 right;
            Vector3 forward;

            if (moveInLocalSpace && rotationSource != null)
            {
                right = rotationSource.right;
                forward = rotationSource.forward;
            }
            else if (moveInLocalSpace)
            {
                Quaternion yawRot = Quaternion.Euler(0f, targetYaw, 0f);
                right = yawRot * Vector3.right;
                forward = yawRot * Vector3.forward;
            }
            else
            {
                right = Vector3.right;
                forward = Vector3.forward;
            }

            right.y = 0f;
            forward.y = 0f;
            right.Normalize();
            forward.Normalize();

            Vector3 delta = (right * input.x + forward * input.y) * moveSpeed * Time.deltaTime;
            baseTargetPosition += delta;
        }
    }

    private void HandleRotationInput()
    {
        if (!isRotationAllowed) return;

        var kb = Keyboard.current;
        if (kb == null) return;

        float rotInput = 0f;

        if (kb.qKey.isPressed) rotInput -= 1f;
        if (kb.eKey.isPressed) rotInput += 1f;

        if (!Mathf.Approximately(rotInput, 0f))
        {
            targetYaw += rotInput * rotationSpeed * Time.deltaTime;
        }
    }

    private void HandleZoomInput()
    {
        float zoomDelta = 0f;

        var mouse = Mouse.current;
        if (mouse != null)
        {
            float scroll = mouse.scroll.ReadValue().y;
            if (!Mathf.Approximately(scroll, 0f))
                zoomDelta -= scroll * zoomSpeed * Time.deltaTime * 50f;
        }

        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.zKey.isPressed) zoomDelta -= zoomKeySpeed * Time.deltaTime;
            if (kb.xKey.isPressed) zoomDelta += zoomKeySpeed * Time.deltaTime;
        }

        if (!Mathf.Approximately(zoomDelta, 0f))
        {
            targetZoomDistance = Mathf.Clamp(targetZoomDistance + zoomDelta, minZoomDistance, maxZoomDistance);
        }
    }

    private void ApplySmoothedTransform()
    {
        currentYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref rotationVelocity, rotationSmoothTime);

        FollowPoint.position = Vector3.SmoothDamp(
            FollowPoint.position,
            baseTargetPosition,
            ref moveVelocity,
            moveSmoothTime
        );

        FollowPoint.rotation = Quaternion.Euler(0f, currentYaw, 0f);

        currentZoomDistance = Mathf.SmoothDamp(
            currentZoomDistance,
            targetZoomDistance,
            ref zoomVelocity,
            zoomSmoothTime
        );

        if (CinemachineCamera != null)
        {
            float t = Mathf.InverseLerp(minZoomDistance, maxZoomDistance, currentZoomDistance);
            targetFOV = Mathf.Lerp(minFOV, maxFOV, t);

            currentFOV = Mathf.SmoothDamp(
                currentFOV,
                targetFOV,
                ref fovVelocity,
                fovSmoothTime
            );

            var lens = CinemachineCamera.Lens;
            lens.OrthographicSize = currentFOV;
            CinemachineCamera.Lens = lens;
        }
    }

    public void SnapToTargets()
    {
        currentYaw = targetYaw = FollowPoint.eulerAngles.y;
        FollowPoint.rotation = Quaternion.Euler(0f, currentYaw, 0f);

        currentZoomDistance = targetZoomDistance;
        moveVelocity = Vector3.zero;
        rotationVelocity = 0f;
        zoomVelocity = 0f;
        fovVelocity = 0f;

        if (CinemachineCamera != null)
        {
            var lens = CinemachineCamera.Lens;
            lens.OrthographicSize = currentFOV;
            CinemachineCamera.Lens = lens;
        }
    }
}

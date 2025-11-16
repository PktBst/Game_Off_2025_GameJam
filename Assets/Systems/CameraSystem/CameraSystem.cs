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
    [Tooltip("How long it takes (approx) to reach the target position. Smaller = snappier.")]
    [SerializeField] private float moveSmoothTime = 0.12f;

    [Header("Rotation")]
    [SerializeField] private bool isRotationAllowed = false;
    [SerializeField] private float rotationSpeed = 90f;
    [Tooltip("How long it takes (approx) to reach the target yaw. Smaller = snappier.")]
    [SerializeField] private float rotationSmoothTime = 0.12f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 6f;                 // scroll wheel sensitivity
    [SerializeField] private float minZoomDistance = -2f;          // used to map to FOV (not used for FollowPoint movement)
    [SerializeField] private float maxZoomDistance = 20f;          // used to map to FOV
    [Tooltip("How long it takes (approx) to reach the target zoom value. Smaller = snappier.")]
    [SerializeField] private float zoomSmoothTime = 0.12f;
    [SerializeField] private float zoomKeySpeed = 4f;              // keyboard Z/X

    [Header("Cinemachine FOV")]
    [SerializeField] private float minFOV = 40f;   // FOV when "zoomed in"
    [SerializeField] private float maxFOV = 70f;   // FOV when "zoomed out"
    [SerializeField] private float fovSmoothTime = 0.1f;

    // Movement smoothing state
    private Vector3 baseTargetPosition;
    private Vector3 moveVelocity = Vector3.zero;

    // Rotation smoothing state
    private float targetYaw;
    private float currentYaw;
    private float rotationVelocity = 0f;

    // Zoom (scalar) used only to drive FOV mapping
    private float targetZoomDistance;
    private float currentZoomDistance;
    private float zoomVelocity = 0f;

    // FOV smoothing state
    private float fovVelocity = 0f;
    private float currentFOV;
    private float targetFOV;

    void Start()
    {
        if (FollowPoint == null)
        {
            Debug.LogError("CameraSystem: FollowPoint not assigned.");
            enabled = false;
            return;
        }

        baseTargetPosition = FollowPoint.position;
        currentYaw = targetYaw = FollowPoint.eulerAngles.y;

        currentZoomDistance = targetZoomDistance = 0f;

        if (CinemachineCamera != null)
        {
            currentFOV = targetFOV = CinemachineCamera.Lens.FieldOfView;
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

    #region Input
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
            Vector3 right = moveInLocalSpace ? Quaternion.Euler(0, targetYaw, 0) * Vector3.right : Vector3.right;
            Vector3 forward = moveInLocalSpace ? Quaternion.Euler(0, targetYaw, 0) * Vector3.forward : Vector3.forward;

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

        // ONLY keyboard Q / E control rotation (affects Y axis only)
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
    #endregion

    #region Smooth apply
    private void ApplySmoothedTransform()
    {
        // Smooth yaw (applied only to Y axis)
        currentYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref rotationVelocity, rotationSmoothTime);
        Quaternion yawRot = Quaternion.Euler(0f, currentYaw, 0f);

        // Smooth movement of FollowPoint (unchanged by zoom)
        Vector3 desiredPos = baseTargetPosition; // zoom no longer offsets FollowPoint
        FollowPoint.position = Vector3.SmoothDamp(FollowPoint.position, desiredPos, ref moveVelocity, moveSmoothTime);

        // Apply rotation (Y axis only)
        FollowPoint.rotation = Quaternion.Euler(0f, currentYaw, 0f);

        // Smooth zoom scalar (used only to compute FOV)
        currentZoomDistance = Mathf.SmoothDamp(currentZoomDistance, targetZoomDistance, ref zoomVelocity, zoomSmoothTime);

        // -----------------
        // Smooth FOV (based on currentZoomDistance)
        // -----------------
        if (CinemachineCamera != null)
        {
            float t = Mathf.InverseLerp(minZoomDistance, maxZoomDistance, currentZoomDistance);
            targetFOV = Mathf.Lerp(minFOV, maxFOV, t);

            currentFOV = Mathf.SmoothDamp(currentFOV, targetFOV, ref fovVelocity, fovSmoothTime);

            // Lens is a struct -> modify copy -> assign back
            var lens = CinemachineCamera.Lens;
            lens.FieldOfView = currentFOV;
            CinemachineCamera.Lens = lens;
        }
    }
    #endregion

    public void SnapToTargets()
    {
        currentYaw = targetYaw = FollowPoint.eulerAngles.y;
        FollowPoint.rotation = Quaternion.Euler(0f, currentYaw, 0f);

        // keep current zoom scalar as-is
        currentZoomDistance = targetZoomDistance;
        moveVelocity = Vector3.zero;
        rotationVelocity = 0f;
        zoomVelocity = 0f;
        fovVelocity = 0f;

        if (CinemachineCamera != null)
        {
            var lens = CinemachineCamera.Lens;
            lens.FieldOfView = currentFOV;
            CinemachineCamera.Lens = lens;
        }
    }
}

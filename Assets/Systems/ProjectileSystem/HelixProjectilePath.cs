using UnityEngine;

[CreateAssetMenu(
    fileName = "HelixProjectilePath",
    menuName = "Projectiles/Paths/Helix Path",
    order = 4)]
public class HelixProjectilePath : ProjectileBehavior
{
    [Header("Helix Settings")]
    [SerializeField] private float turns = 4f;    // Number of spiral turns
    [SerializeField] private float radius = 0.5f; // Spiral radius

    [Header("Path Curvature")]
    [SerializeField] private float pathAmplitude = 0.5f;  // Max distance from straight line
    [SerializeField] private Vector3 pathDirection = Vector3.up; // Direction to bend along path

    private Vector3 fallbackAxis = Vector3.right;

    public override Vector3 LerpFunc(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);

        Vector3 basePos = Vector3.Lerp(start, end, t);

        Vector3 forward = (end - start).normalized;
        if (forward.sqrMagnitude < 0.0001f)
            return basePos;

        // Perpendicular axes for helix
        Vector3 right = Vector3.Cross(forward, Vector3.up);
        if (right.sqrMagnitude < 0.0001f)
            right = Vector3.Cross(forward, fallbackAxis);

        right.Normalize();
        Vector3 up = Vector3.Cross(right, forward).normalized;

        // Helix offset
        float angle = t * Mathf.PI * 2f * turns;   // rotation angle
        float taper = Mathf.Sin(t * Mathf.PI);      // fade at start/end
        Vector3 helixOffset = Mathf.Cos(angle) * right + Mathf.Sin(angle) * up;

        // Path curvature (parabolic / directional)
        Vector3 pathCurve = Vector3.ProjectOnPlane(pathDirection, forward).normalized * pathAmplitude * taper;

        // Combine helix and path curve
        return basePos + helixOffset * radius * taper + pathCurve;
    }
}

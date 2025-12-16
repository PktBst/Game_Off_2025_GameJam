using UnityEngine;

[CreateAssetMenu(
    fileName = "CurvedProjectilePath",
    menuName = "Projectiles/Paths/Curved Path",
    order = 2)]
public class CurvedProjectilePath : ProjectileBehavior
{
    [Header("Curve Settings")]

    // Maximum distance the projectile deviates from the straight path
    [SerializeField] private float amplitude = 1f;

    // Direction of curvature (world-space hint)
    [SerializeField] private Vector3 direction = Vector3.up;

    private Vector3 fallbackAxis = Vector3.right;

    public override Vector3 LerpFunc(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);

        Vector3 basePos = Vector3.Lerp(start, end, t);

        Vector3 forward = (end - start).normalized;
        if (forward.sqrMagnitude < 0.0001f)
            return basePos;

        // Build stable local axes
        Vector3 right = Vector3.Cross(forward, Vector3.up);
        if (right.sqrMagnitude < 0.0001f)
            right = Vector3.Cross(forward, fallbackAxis);

        right.Normalize();
        Vector3 up = Vector3.Cross(right, forward).normalized;

        // Project direction into the perpendicular plane
        Vector3 curvePlaneDir =
            Vector3.ProjectOnPlane(direction, forward);

        if (curvePlaneDir.sqrMagnitude < 0.0001f)
            curvePlaneDir = up; // safe default

        curvePlaneDir.Normalize();

        float curve = Mathf.Sin(t * Mathf.PI);

        return basePos + curvePlaneDir * amplitude * curve;
    }
}

using UnityEngine;

[CreateAssetMenu(
    fileName = "ZigzagProjectilePath",
    menuName = "Projectiles/Paths/Zigzag Path",
    order = 3)]
public class ZigzagProjectilePath : ProjectileBehavior
{
    [Header("Zigzag Settings")]
    [SerializeField] private int zigZagCount = 4;      // Number of zigzags along the path
    [SerializeField] private float amplitude = 0.5f;   // Max deviation from center
    [SerializeField] private Vector3 zigZagDirection = Vector3.right; // Direction of zigzag (world-space)

    [Header("Path Curvature")]
    [SerializeField] private float pathAmplitude = 0.5f;         // Curve offset magnitude
    [SerializeField] private Vector3 pathDirection = Vector3.up; // Direction of path curve

    private Vector3 fallbackAxis = Vector3.right;

    public override Vector3 LerpFunc(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);

        Vector3 basePos = Vector3.Lerp(start, end, t);

        Vector3 forward = (end - start).normalized;
        if (forward.sqrMagnitude < 0.0001f)
            return basePos;

        // Project zigzag direction onto plane perpendicular to forward
        Vector3 zigzagDir = Vector3.ProjectOnPlane(zigZagDirection, forward);
        if (zigzagDir.sqrMagnitude < 0.0001f)
        {
            zigzagDir = Vector3.Cross(forward, fallbackAxis); // fallback
        }
        zigzagDir.Normalize();

        // Zigzag wave
        float wave = Mathf.Sin(t * Mathf.PI * 2f * zigZagCount);
        float fade = Mathf.Sin(t * Mathf.PI);
        Vector3 zigzagOffset = zigzagDir * wave * fade * amplitude;

        // Path curvature (like curved projectile)
        Vector3 curveDir = Vector3.ProjectOnPlane(pathDirection, forward);
        if (curveDir.sqrMagnitude < 0.0001f)
            curveDir = zigzagDir; // fallback
        curveDir.Normalize();
        Vector3 pathCurve = curveDir * pathAmplitude * fade;

        return basePos + zigzagOffset + pathCurve;
    }
}

using UnityEngine;

public class ZigzagProjectilePath : ProjectileBehavior
{
    public override Vector3 LerpFunc(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);

        // Base interpolation
        Vector3 basePos = Vector3.Lerp(start, end, t);

        // Direction
        Vector3 dir = (end - start).normalized;

        // Perpendicular direction
        Vector3 perp = Vector3.Cross(dir, Vector3.up);
        if (perp.sqrMagnitude < 0.0001f)
            perp = Vector3.Cross(dir, Vector3.right);

        perp.Normalize();

        // Zig-zag wave (smooth)
        float zigZag = Mathf.Sin(t * Mathf.PI * 8f); // 4 zig-zags

        // Amplitude scales down near start & end
        float amplitude = Mathf.Sin(t * Mathf.PI) * 0.5f;

        return basePos + perp * zigZag * amplitude;
    }

}

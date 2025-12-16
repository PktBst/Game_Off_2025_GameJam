using UnityEngine;

public class HelixProjectilePath : ProjectileBehavior
{
    public override Vector3 LerpFunc(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);

        // Base interpolation
        Vector3 basePos = Vector3.Lerp(start, end, t);

        // Forward direction
        Vector3 dir = (end - start).normalized;

        // Build perpendicular basis vectors
        Vector3 right = Vector3.Cross(dir, Vector3.up);
        if (right.sqrMagnitude < 0.0001f)
            right = Vector3.Cross(dir, Vector3.right);

        right.Normalize();
        Vector3 up = Vector3.Cross(dir, right);

        // Helix parameters
        float turns = 4f;                 // number of spiral turns
        float angle = t * Mathf.PI * 2f * turns;

        // Radius tapers near start & end
        float radius = Mathf.Sin(t * Mathf.PI) * 0.5f;

        // Circular offset around direction
        Vector3 helixOffset =
            Mathf.Cos(angle) * right +
            Mathf.Sin(angle) * up;

        return basePos + helixOffset * radius;
    }
}

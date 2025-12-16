using UnityEngine;

[CreateAssetMenu(
    fileName = "StraightProjectilePath",
    menuName = "Projectiles/Paths/Straight Path",
    order = 1)]
public class StraightProjectilePath : ProjectileBehavior
{
    [Header("Optional Offsets")]
    [SerializeField] private float horizontalOffset = 0f;
    [SerializeField] private float verticalOffset = 0f;
    
    private Vector3 fallbackAxis = Vector3.right;

    public override Vector3 LerpFunc(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);

        Vector3 basePos = Vector3.Lerp(start, end, t);

        Vector3 dir = (end - start).normalized;
        if (dir.sqrMagnitude < 0.0001f)
            return basePos;

        Vector3 right = Vector3.Cross(dir, Vector3.up);
        if (right.sqrMagnitude < 0.0001f)
            right = Vector3.Cross(dir, fallbackAxis);

        right.Normalize();
        Vector3 up = Vector3.Cross(dir, right);

        return basePos +
               right * horizontalOffset +
               up * verticalOffset;
    }
}

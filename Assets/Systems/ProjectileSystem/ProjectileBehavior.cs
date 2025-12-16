using UnityEngine;

[System.Serializable]

public class ProjectileBehavior : ScriptableObject
{
    public virtual Vector3 LerpFunc(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);
        Debug.Log("<color = red>Base Projectile Behavior</color>");
        return Vector3.Lerp(start,end,t);
    }
}

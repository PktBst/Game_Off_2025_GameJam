using UnityEngine;

public class BuffEffect : MonoBehaviour
{
    protected virtual void Awake()
    {
        AnimationPool.Instance.Play_CFXR3_LightGlow_A_Loop__AnimationAtFor(transform.forward,transform.position,3f);
    }
}

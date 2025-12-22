using UnityEngine;

public class FireDamageBuff : BuffEffect
{
    public GameObject FireEffect;
    public override void Init()
    {
        base.Init();
        if (TryGetComponent(out AttackComponent attackComponent))
        {
            attackComponent.projectileModel = FireEffect;
        }
    }
}

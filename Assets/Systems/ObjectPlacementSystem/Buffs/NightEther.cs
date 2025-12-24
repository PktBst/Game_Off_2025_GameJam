using UnityEngine;

public class NightEther : BuffEffect
{
    public override void Init()
    {
        if(TryGetComponent(out HealthComponent healthComponent))
        {
            healthComponent.AddHealth(healthComponent.MaxHealth*0.2f);
        }
    }
}

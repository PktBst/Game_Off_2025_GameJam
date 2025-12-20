using UnityEngine;

public class ExplodeOnDeath : BuffEffect
{
    HealthComponent health;

    protected override void Awake()
    {
        base.Awake();
        if (TryGetComponent(out health))
        {
            health.OnDeath += ExplodeDeath;
        }        
    }
    private void ExplodeDeath()
    {
        Debug.Log("Exploded On Death");
    }
}

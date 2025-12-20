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

        var colliders = Physics.OverlapSphere(transform.position, 2f);
        AnimationPool.Instance.Play_WFXMR_Explosion_AnimationAtFor(transform.forward, transform.position, 0.7f);
        foreach (var collider in colliders)
        {
            if(collider.TryGetComponent(out HealthComponent targetHealth) && TryGetComponent(out StatsComponent stats) && targetHealth.Stats.FactionType!=stats.FactionType)
            {
                targetHealth.DeductHealth(targetHealth.CurrentHealth);
            } 
        }
    }
}

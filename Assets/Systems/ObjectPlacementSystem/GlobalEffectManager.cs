using System;
using UnityEngine;

public class GlobalEffectManager : MonoBehaviour
{
    public Action<Vector3> OnDeathEffect;
    public GameObject ExplodeEffect;
    public GlobalEffectManager Instance;
    private void Awake()
    {
        Instance = this; 
        OnDeathEffect += ExplodeOnDeath; 
    }
    void ExplodeOnDeath(Vector3 ExplodeEffectSpawnLocation)
    {
        Collider[] hitColliders = Physics.OverlapSphere(ExplodeEffectSpawnLocation, 1);

        Destroy(ExplodeEffect,1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<HealthComponent>(out HealthComponent health))
            {
                health.DeductHealth(100f);
            }
        }
    }
}

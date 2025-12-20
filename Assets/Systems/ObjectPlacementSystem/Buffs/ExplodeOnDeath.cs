using UnityEngine;

public class ExplodeOnDeath : CardData, IBuffs
{
    HealthComponent health;

    public override void Init(CardData cardData)
    {
        base.Init(cardData);
    }

    private void Awake()
    {
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

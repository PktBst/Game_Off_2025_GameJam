using UnityEngine;

public class StatsComponent : MonoBehaviour
{
    public Faction FactionType;

    public float BaseAttackPoints;
    public float BaseAttackCooldown;
    public float BaseAttackAccuracy;
    public float BaseAttackRange;
    public int TaxAmount;

    public float BaseDefensePoints;
    private bool hasSurvivedOneNight;

    public void Init(float AttackDamage, float AttackRange, float Cooldown, Faction faction = Faction.GoodGuys)
    {
        BaseAttackPoints = AttackDamage;
        BaseAttackCooldown = Cooldown;
        BaseAttackRange = AttackRange;
        FactionType = faction;
        hasSurvivedOneNight = false;

        if (DayNightCycleCounter.Instance != null && DayNightCycleCounter.Instance.PayTaxesOnDay)
        {
            DayNightCycleCounter.Instance.OnTimeOfDayChange += PayTax;
        }
    }
    private void OnDestroy()
    {
        if (DayNightCycleCounter.Instance != null && DayNightCycleCounter.Instance.PayTaxesOnDay)
        {
            DayNightCycleCounter.Instance.OnTimeOfDayChange -= PayTax;
        }
    }

    void PayTax(TimeOfDay time)
    {
        if (time == TimeOfDay.Night || DayNightCycleCounter.Instance == null)
        {
            hasSurvivedOneNight = true;
            return;
        }
        if (!hasSurvivedOneNight)
        {
            return;
        }

        float healthRatio = 1f;

        if (TryGetComponent(out HealthComponent health))
        {
            healthRatio = Mathf.Clamp01(health.CurrentHealth / health.MaxHealth);
        }

        int taxAmount = Mathf.RoundToInt(TaxAmount * healthRatio);

        for(int i =0; i < taxAmount; i++)
        {
            StartCoroutine(SpawnCoins(taxAmount));
        }
    }
    System.Collections.IEnumerator SpawnCoins(int amt)
    {
        for(int i = 0;i< amt;i++)
        {
            if (CurrencySystem.TryGetCoin(transform.position, out var coin))
            {
                CurrencySystem.TryAddAmount(amt);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }


}

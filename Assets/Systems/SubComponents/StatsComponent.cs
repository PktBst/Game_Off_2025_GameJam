using UnityEngine;

public class StatsComponent : MonoBehaviour
{
    public Faction FactionType;

    public float BaseAttackPoints;
    public float BaseAttackCooldown;
    public float BaseAttackAccuracy;
    public float BaseAttackRange;


    public float BaseDefensePoints;


    public void Init(float AttackDamage, float AttackRange, float Cooldown, Faction faction = Faction.GoodGuys)
    {
        BaseAttackPoints = AttackDamage;
        BaseAttackCooldown = Cooldown;
        BaseAttackRange = AttackRange;
        FactionType = faction;

    }

}

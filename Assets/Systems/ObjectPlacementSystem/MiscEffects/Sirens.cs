using UnityEngine;
using UnityEngine.AI;

public class Sirens : Cavemen
{
    public override bool TryUsingThisCard()
    {
        var currentTile = GridSystem.Instance.CurrentTile;
        if (currentTile == null || currentTile.IsBlocked) return false;
        squadPost = Instantiate(squadPostPrefab, currentTile.Pos, Quaternion.identity);
        ComponentUtility.AddComponentWithValues(squadPost.gameObject, this).SpawnUnits(currentTile);
        return true;
    }

    public override void Init()
    {

    }
    protected override void SpawnUnits(Tile currentTile)
    {
        base.SpawnUnits(currentTile);
    }
    public override NavMeshAgent SpawnUnitAt(Vector3 spawnPos)
    {
        var spawnedUnit = base.SpawnUnitAt(spawnPos);
        if(spawnedUnit.TryGetComponent(out SquadUnitAttackComponent squadUnitAttackComponent))
        {
            squadUnitAttackComponent.OnEnemyDetected += OnSearchForEnemy;
        }
        return spawnedUnit;
    }

    void OnSearchForEnemy(Transform self,Transform enemy)
    {
      
        var hits = Physics.OverlapSphere(self.position,5f);

        var selfHealth = self.GetComponent<HealthComponent>();
        foreach (var target in hits)
        {
            if (target.TryGetComponent(out AttackComponent targetAttack) && target.TryGetComponent(out StatsComponent targetStats) && targetStats.FactionType!= selfHealth.Stats.FactionType)
            {
                targetAttack.targetHealth = selfHealth; // lure duration
            }
        }
    }

}
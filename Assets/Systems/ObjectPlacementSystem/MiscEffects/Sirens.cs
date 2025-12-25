using UnityEngine;
using UnityEngine.AI;

public class Sirens : Cavemen
{
    float duration = 0.25f;
    float elapsed = 0;
    public override bool TryUsingThisCard()
    {
        var currentTile = GridSystem.Instance.CurrentTile;
        if (currentTile == null || currentTile.IsBlocked) return false;
        squadPost = Instantiate(squadPostPrefab, currentTile.Pos, Quaternion.identity);
        var spawnedScipt = ComponentUtility.AddComponentWithValues(squadPost.gameObject, this);
        spawnedScipt.SpawnUnits(currentTile);
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
        return spawnedUnit;
    }

    private void Update()
    {
        if(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            return;
        }
        elapsed = 0;
        OnSearchForEnemy();
    }

    void OnSearchForEnemy()
    {
      
        if(squadPost.UnitList ==null || squadPost.UnitList.Count == 0)
        {
            return;
        }
        var rand = Random.Range(0, squadPost.UnitList.Count);
        var self = squadPost.UnitList[rand];
        var hits = Physics.OverlapSphere(squadPost.SquadPostPiller.position,squadPost.followTriggerRange+2);

        var selfHealth = self.GetComponent<HealthComponent>();

        foreach (var target in hits)
        {
            if (target.TryGetComponent(out AttackComponent targetAttack) && target.TryGetComponent(out StatsComponent targetStats) && targetStats.FactionType!= selfHealth.Stats.FactionType)
            {
                targetAttack.ForceLockTarget(selfHealth); // lure duration
            }
        }
    }

}
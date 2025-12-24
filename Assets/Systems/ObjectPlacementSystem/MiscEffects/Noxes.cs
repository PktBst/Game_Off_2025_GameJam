using UnityEngine;
using UnityEngine.AI;

public class Noxes : Cavemen
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
        if(spawnedUnit.TryGetComponent(out SquadUnitAttackComponent attackComponent))
        {
            attackComponent.AttackAction = OnTargetAttack;
        }
        return spawnedUnit;
    }

    void OnTargetAttack(HealthComponent targetHealthComponent)
    {
        if(targetHealthComponent != null && targetHealthComponent.TryGetComponent(out AttackComponent targetAttack))
        {
            targetAttack.Dazed = true;
        } 
    }
}

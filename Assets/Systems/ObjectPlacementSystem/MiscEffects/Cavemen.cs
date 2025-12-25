using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
/// <summary>
/// This card Spawns 5 cavemen
/// </summary>
public class Cavemen : MiscEffect
{
    [SerializeField] protected NavMeshAgent unitPrefab;
    [SerializeField] protected SquadPost squadPostPrefab;

    protected SquadPost squadPost;

    public SquadPost Post => squadPost;
    public event System.Action<NavMeshAgent> OnUnitDie;
    int BaseCount = 5;
    public int AdditionCount;

    public override bool TryUsingThisCard()
    {
        var currentTile = GridSystem.Instance.CurrentTile;
        if(currentTile==null || currentTile.IsBlocked) return false;
        squadPost = Instantiate(squadPostPrefab, currentTile.Pos, Quaternion.identity);
        ComponentUtility.AddComponentWithValues(squadPost.gameObject,this).SpawnUnits(currentTile);
        return true;
    }

    public override void Init()
    {
        
    }
    protected Vector3 GetRandomSpawnOffset()
    {
        // Random direction
        Vector2 dir = Random.insideUnitCircle.normalized;

        // Random distance between 0.15 and 1 (exclusive)
        float distance = Random.Range(0.15f, 1f);

        return new Vector3(dir.x, 0f, dir.y) * distance;
    }

    protected virtual void SpawnUnits(Tile currentTile)
    {
        foreach (var unit in squadPost.UnitList)
        {
            Destroy(unit.gameObject);
        }
        squadPost.UnitList = new();
        for (int i = 0; i < BaseCount + AdditionCount; i++)
        {
            Vector3 spawnPos = currentTile.Pos + GetRandomSpawnOffset();
            SpawnUnitAt(spawnPos);
        }

        if (DayNightCycleCounter.Instance != null)
        {
            DayNightCycleCounter.Instance.OnTimeOfDayChange += OnDayChange;
        }
    }
    protected void OnDayChange(TimeOfDay time)
    {
        if (time == TimeOfDay.Night)
        {
            return;
        }
        int currentcnt = 0;
        foreach (var unit in squadPost.UnitList)
        {
            if(unit.TryGetComponent(out HealthComponent health))
            {
                health.AddHealth(health.MaxHealth);
            }
            currentcnt++;
        }
        for (int i = currentcnt; i < BaseCount + AdditionCount; i++)
        {
            Vector3 spawnPos = squadPost.SquadPostPiller.position + GetRandomSpawnOffset();
            SpawnUnitAt(spawnPos);
        }
    }
    public virtual NavMeshAgent SpawnUnitAt(Vector3 spawnPos)
    {
        var spawnedUnit = Instantiate(unitPrefab, spawnPos, Quaternion.identity, transform);

        if (spawnedUnit.TryGetComponent(out HealthComponent health))
        {
            health.OnDeath += () =>
            {
                squadPost.UnitList.Remove(spawnedUnit);
                OnUnitDie?.Invoke(spawnedUnit);
            };
        }

        squadPost.UnitList.Add(spawnedUnit);
        return spawnedUnit;
    }
}
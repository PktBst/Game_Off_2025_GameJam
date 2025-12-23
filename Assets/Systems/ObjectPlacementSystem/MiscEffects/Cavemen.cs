using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using Unity.VisualScripting;
/// <summary>
/// This card Spawns 5 cavemen
/// </summary>
public class Cavemen : MiscEffect
{
    [SerializeField] NavMeshAgent unitPrefab;
    [SerializeField] SquadPost squadPostPrefab;

    SquadPost squadPost;

    int BaseCount = 5;
    public int AdditionCount;
    Vector3[] spawnOffsets =
    {
        Vector3.forward * 0.5f,
        Vector3.back * 0.5f,
        Vector3.left * 0.5f,
        Vector3.right * 0.5f
    };

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
    Vector3 GetRandomSpawnOffset()
    {
        // Random direction
        Vector2 dir = Random.insideUnitCircle.normalized;

        // Random distance between 0.15 and 1 (exclusive)
        float distance = Random.Range(0.15f, 1f);

        return new Vector3(dir.x, 0f, dir.y) * distance;
    }

    private void SpawnUnits(Tile currentTile)
    {
        foreach (var unit in squadPost.UnitList)
        {
            Destroy(unit.gameObject);
        }
        squadPost.UnitList = new();
        for (int i = 0; i < BaseCount + AdditionCount; i++)
        {
            Vector3 spawnPos = currentTile.Pos + GetRandomSpawnOffset();

            var spawnedUnit = Instantiate(
                unitPrefab,
                spawnPos,
                Quaternion.identity,
                squadPost.transform
            );

            if (spawnedUnit.TryGetComponent(out HealthComponent health))
            {
                health.OnDeath += () =>
                {
                    squadPost.UnitList.Remove(spawnedUnit);
                };
            }

            squadPost.UnitList.Add(spawnedUnit);
        }

        if (DayNightCycleCounter.Instance != null)
        {
            DayNightCycleCounter.Instance.OnTimeOfDayChange += OnDayChange;
        }
    }
    void OnDayChange(TimeOfDay time)
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
            Vector3 spawnPos = squadPost.transform.position + GetRandomSpawnOffset();

            var spawnedUnit = Instantiate(unitPrefab, spawnPos, Quaternion.identity,transform);

            if (spawnedUnit.TryGetComponent(out HealthComponent health))
            {
                health.OnDeath += () =>
                {
                    squadPost.UnitList.Remove(spawnedUnit);
                };
            }

            squadPost.UnitList.Add(spawnedUnit);
        }
    }
}

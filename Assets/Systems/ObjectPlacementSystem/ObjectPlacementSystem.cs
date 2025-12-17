using System;
using UnityEngine;

public class ObjectPlacementSystem : MonoBehaviour
{
    [SerializeField] GameObject placeableObjectSkeletonPrefab;
    [SerializeField] PlaceableObjectDB_SO placeableObjectDB;

    private static EPlaceableObjectType[] enumValues;

    private void Awake()
    {
        enumValues = (EPlaceableObjectType[])Enum.GetValues(typeof(EPlaceableObjectType));
    }
    public bool SpawnPlaceableObjectAtTile(Tile tile, EPlaceableObjectType type)
    {
        if (tile.IsBlocked) return false;

        Collider[] colliders = Physics.OverlapBox(tile.Pos, Vector3.one*0.5f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<MoveComponent>(out var move))
            {
                Vector3 moveDir = (move.transform.position - (Vector3)tile.Pos).normalized;
                Vector3 moveTo = move.transform.position;
                while (GridSystem.GetTileByWorldPosition(moveTo) == tile)
                {
                    moveTo += moveDir;
                }
                while (GridSystem.GetTileByWorldPosition(moveTo).IsBlocked)
                {
                    moveTo += moveDir;
                }
                move.MoveTo(moveTo);
            }
        }

        PlaceableObject_SO obj = placeableObjectDB.GetPlaceableObjectByType(type);

        tile.OccupyingEntity = Instantiate(placeableObjectSkeletonPrefab, tile.Pos, Quaternion.identity);
        tile.OccupyingEntity.GetComponent<StatsComponent>().Init(obj.AttackDamage, obj.AttackRange, obj.Cooldown);
        tile.OccupyingEntity.GetComponent<HealthComponent>().Init(obj.BaseHealth);

        tile.OccupyingEntity.GetComponent<AttackComponent>()
            .Init(obj.Behaviour.ExecuteBehaviour);

        tile.OccupyingEntity.GetComponent<VisualComponent>().Init(obj.GameModel);

        Debug.Log($"Spawned a <color=red>{obj.name}</color>");
        return true;
    }

    public PlaceableObject_SO GetRandomPlaceableObject()
    {
        return placeableObjectDB.GetPlaceableObjectByType(enumValues[UnityEngine.Random.Range(0, enumValues.Length)]);
    }
}
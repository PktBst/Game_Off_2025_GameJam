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
    public bool SpawnPlaceableObjectAtTile(Tile tile, string name)
    {
        GameObject obj = placeableObjectDB.GetPlaceableObjectByType(name);
        if (tile.IsBlocked && obj.GetComponent<CardData>().CardType == CardType.Building) 
        { 
            return false; 
        }

        moveUnitsAwayFromSpawnPoint(tile);
        doTurnBasedGameModeThings();

        //tile.OccupyingEntity = Instantiate(placeableObjectSkeletonPrefab, tile.Pos, Quaternion.identity);
        if(obj.GetComponent<CardData>().CardType == CardType.Building)tile.OccupyingEntity = Instantiate(obj, tile.Pos, Quaternion.identity);
        else if (obj.GetComponent<CardData>().CardType==CardType.Buff)
        {
            var source = obj.GetComponent<BuffEffect>();
            if (tile.OccupyingEntity != null)
            {
                tile.OccupyingEntity
                    .AddComponent(source.GetType());

                return true;
            }

            return false;
        }
        //tile.OccupyingEntity.GetComponent<StatsComponent>().TaxAmount = obj.BaseTaxAmount;
        //tile.OccupyingEntity.GetComponent<StatsComponent>().Init(obj.AttackDamage, obj.AttackRange, obj.Cooldown);
        //tile.OccupyingEntity.GetComponent<HealthComponent>().Init(obj.BaseHealth);

        //tile.OccupyingEntity.GetComponent<AttackComponent>()
        //    .Init(obj.Behaviour.ExecuteBehaviour);

        //tile.OccupyingEntity.GetComponent<VisualComponent>().Init(obj.GameModel);

        //Debug.Log($"Spawned a <color=red>{obj}</color>");
        return true;
    }

    public GameObject GetRandomPlaceableObject()
    {
        return placeableObjectDB.GetRandomObject();
    }

    void doTurnBasedGameModeThings()
    {
        if (GameManager.Instance.PlayThisGameMode != GameModeType.TurnBased) return;
        TurnBasedGameMode gm = GameManager.Instance.CurrentGameMode as TurnBasedGameMode;
        gm.EndCurrentTurn();
    }
    void moveUnitsAwayFromSpawnPoint(Tile tile)
    {
        Collider[] colliders = Physics.OverlapBox(tile.Pos, Vector3.one * 0.5f);
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
    }
}

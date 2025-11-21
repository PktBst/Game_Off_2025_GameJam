using UnityEngine;

public class ObjectPlacementSystem : MonoBehaviour
{
    [SerializeField] PlaceableObjectDB_SO placeableObjectDB;

    public void SpawnPlaceableObjectAtTile(Tile tile, EPlaceableObjectType type)
    {
        if (tile.IsBlocked) return;
        PlaceableObject placeableObj = placeableObjectDB.GetPlaceableObjectByType(type);
        tile.OccupyingEntity = Instantiate(placeableObj.GameModel, tile.Pos, Quaternion.identity);
        tile.OccupyingEntity.GetComponent<HealthComponent>().Init(placeableObj.BaseLifeTime,Faction.GoodGuys);
    }
}
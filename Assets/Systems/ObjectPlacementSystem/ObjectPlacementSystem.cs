using System;
using UnityEngine;

public class ObjectPlacementSystem : MonoBehaviour
{
    [SerializeField] PlaceableObjectDB_SO placeableObjectDB;

    private static EPlaceableObjectType[] enumValues;

    private void Awake()
    {
        enumValues = (EPlaceableObjectType[])Enum.GetValues(typeof(EPlaceableObjectType));
    }
    public void SpawnPlaceableObjectAtTile(Tile tile, EPlaceableObjectType type)
    {
        if (tile.IsBlocked) return;
        PlaceableObject placeableObj = placeableObjectDB.GetPlaceableObjectByType(type);
        tile.OccupyingEntity = Instantiate(placeableObj.GameModel, tile.Pos, Quaternion.identity);
        tile.OccupyingEntity.GetComponent<HealthComponent>().Init(placeableObj.BaseLifeTime,Faction.GoodGuys);
    }

    public PlaceableObject GetRandomPlaceableObject()
    {
        return placeableObjectDB.GetPlaceableObjectByType(enumValues[UnityEngine.Random.Range(0, enumValues.Length)]);
    }
}
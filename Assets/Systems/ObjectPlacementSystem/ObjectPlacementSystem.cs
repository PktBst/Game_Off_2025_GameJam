using UnityEngine;

public class ObjectPlacementSystem : MonoBehaviour
{
    [SerializeField] PlaceableObjectDB_SO placeableObjectDB;

    public void SpawnPlaceableObjectAtTile(Tile tile, EPlaceableObjectType type)
    {
        if (tile.IsBlocked) return;
        PlaceableObj placeableObj = placeableObjectDB.GetPlaceableObjectByType(type);
        tile.OccupyingEntity = Instantiate(placeableObj.GameModel, tile.Pos, Quaternion.identity);
    }
}

[System.Serializable]
public enum EPlaceableObjectType
{
    //Attack
    FireArcherTower,
}

[System.Serializable]
public class PlaceableObj
{
    [SerializeField] public GameObject GameModel;
    [SerializeField] public EPlaceableObjectType Type;
}
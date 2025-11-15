using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceableObjectDB_SO", menuName = "Scriptable Objects/PlaceableObjectDB_SO")]
public class PlaceableObjectDB_SO : ScriptableObject
{
    [SerializeField] public List<PlaceableObj> AllPlaceableObject;

    public PlaceableObj GetPlaceableObjectByType(EPlaceableObjectType type)
    {
        return AllPlaceableObject.Where(t => t.Type == type).FirstOrDefault();
    }
}


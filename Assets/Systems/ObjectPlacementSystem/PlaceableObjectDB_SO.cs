using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceableObjectDB_SO", menuName = "Scriptable Objects/PlaceableObjectDB_SO")]
public class PlaceableObjectDB_SO : ScriptableObject
{
    [SerializeField] public List<PlaceableObject_SO> AllPlaceableObjectSOs;

    public PlaceableObj GetPlaceableObjectByType(EPlaceableObjectType type)
    {
        return AllPlaceableObjectSOs.Where(t => t.PlaceableObjData.Type == type).FirstOrDefault().PlaceableObjData;
    }
}


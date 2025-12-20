using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceableObjectDB_SO", menuName = "Scriptable Objects/PlaceableObjectDB_SO")]
public class PlaceableObjectDB_SO : ScriptableObject
{
    [SerializeField] public List<CardData> AllPlaceableObjectSOs;
    public GameObject GetPlaceableObjectByType(string value)
    {
        foreach(CardData data in AllPlaceableObjectSOs)
        {
            if (data.DisplayName == value) return data.gameObject;
        }
        Debug.LogWarning($"Object with name '{value}' not found in Database.");
        return null;
    }

    public GameObject GetRandomObject()
    {
        if (AllPlaceableObjectSOs.Count == 0) return null;
        return AllPlaceableObjectSOs[Random.Range(0, AllPlaceableObjectSOs.Count)].gameObject;
    }
}
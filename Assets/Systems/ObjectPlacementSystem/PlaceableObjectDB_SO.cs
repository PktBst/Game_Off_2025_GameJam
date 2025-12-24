using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceableObjectDB_SO", menuName = "Scriptable Objects/PlaceableObjectDB_SO")]
public class PlaceableObjectDB_SO : ScriptableObject
{
    [SerializeField] public List<FactionCardCollection> AllPlaceableObjectSOs;
    public GameObject GetPlaceableObjectByType(string value)
    {
        foreach(FactionCardCollection factionCardCollection in AllPlaceableObjectSOs)
        {
            foreach (CardData data in factionCardCollection.CardList)
            {
                if (data.DisplayName == value) return data.gameObject;
            }
        }
        Debug.LogWarning($"Object with name '{value}' not found in Database.");
        return null;
    }

    public GameObject GetRandomObject()
    {
        if (AllPlaceableObjectSOs.Count == 0) return null;
        List<CardData> factionCardCollection = AllPlaceableObjectSOs[Random.Range(0, AllPlaceableObjectSOs.Count)].CardList;
        return factionCardCollection[Random.Range(0, factionCardCollection.Count)].gameObject;
    }

    public List<CardData> GetCardDataByCollection(CardCollection collectionType)
    {
        List<CardData> result = new List<CardData>();

        foreach (FactionCardCollection factionCardCollection in AllPlaceableObjectSOs)
        {
            if (factionCardCollection.CollectionType == collectionType)
            {
                result.AddRange(factionCardCollection.CardList);
            }
        }

        if (result.Count == 0)
        {
            Debug.LogWarning($"No CardData found for collection '{collectionType}'.");
        }

        return result;
    }
}

[System.Serializable]
public class FactionCardCollection
{
    public CardCollection CollectionType;
    public List<CardData> CardList;
}

public enum CardCollection
{
    None,
    KingOfWild,
    KingOfNight
}
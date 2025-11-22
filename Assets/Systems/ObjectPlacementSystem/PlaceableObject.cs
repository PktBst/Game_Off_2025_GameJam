using UnityEngine;


[System.Serializable]
public class PlaceableObject
{
    [SerializeField] public EPlaceableObjectType Type;
    [SerializeField] public float BaseLifeTime;
    [SerializeField] public GameObject GameModel;
}


[System.Serializable]
public enum EPlaceableObjectType
{
    //Attack
    ArcherTower,
    WitchTower,
    PoisonTower,
    BallistaTower,
    CanonTower,
    
}
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceableObject_SO", menuName = "Scriptable Objects/PlaceableObject_SO")]
public class PlaceableObject_SO : ScriptableObject
{
    [SerializeField] public EPlaceableObjectType Type;
    [SerializeField] public float BaseHealth;
    [SerializeField] public GameObject GameModel;
    [SerializeField] public BaseBehaviour_SO Behaviour;
    [SerializeField] public float AttackDamage;
    [SerializeField] public float AttackRange;
    [SerializeField] public float Cooldown;
}
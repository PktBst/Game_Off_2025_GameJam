using UnityEngine;

public class CardData : MonoBehaviour
{
    public string DisplayName;
    public string Desc;
    public int Cost;
    public CardType CardType;

}

public enum CardType
{
    Building,
    Buff,
    Passive,
    Misc
}
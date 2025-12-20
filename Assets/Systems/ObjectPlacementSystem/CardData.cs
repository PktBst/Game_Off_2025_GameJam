using UnityEngine;

public class CardData : MonoBehaviour
{
    public string DisplayName;
    public string Desc;
    public int Cost;
    public CardType CardType;

    public void Init(string displayName, string desc, int cost, CardType cardtype)
    {
        DisplayName = displayName;
        Desc = desc;
        Cost = cost;
        CardType = cardtype;
    }
    public virtual void Init(CardData cardData)
    {
        Init(cardData.name,cardData.Desc, cardData.Cost, cardData.CardType);
    }

}

public enum CardType
{
    Building,
    Buff,
    Passive,
    Misc
}
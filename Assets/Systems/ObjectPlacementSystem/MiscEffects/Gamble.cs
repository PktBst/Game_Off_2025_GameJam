using UnityEngine;

public class Gamble : MiscEffect
{
    public override bool TryUsingThisCard()
    {
        if (GameManager.Instance == null || GameManager.Instance.CardSystem == null) return false;
        GameManager.Instance.CardSystem.DeleteRandomCardInHand(count: 2);
        GameManager.Instance.CardSystem.AddRandomCardInHand(count: 3);
        return true;
    }

    public override void Init()
    {
        
    }
}

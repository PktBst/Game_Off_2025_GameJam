using UnityEngine;

public class GambleEffect : BuffEffect
{
    public override void Init()
    {
        base.Init();

        Debug.Log("Gamble Executed");
        GameManager.Instance.CardSystem.DeleteRandomCardInHand(count: 2);
        GameManager.Instance.CardSystem.AddRandomCardInHand(count: 3);
        //Destroy(this.gameObject);
    }
}

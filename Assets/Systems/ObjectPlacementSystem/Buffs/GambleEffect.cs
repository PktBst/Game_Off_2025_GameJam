using UnityEngine;

public class GambleEffect : BuffEffect
{
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    void Init()
    {
        Debug.Log("Gamble Executed");
        GameManager.Instance.CardSystem.DeleteRandomCardInHand(count: 2);
        GameManager.Instance.CardSystem.AddRandomCardInHand(count: 2);
        //Destroy(this.gameObject);
    }
}

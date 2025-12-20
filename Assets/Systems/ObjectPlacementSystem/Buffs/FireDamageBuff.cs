using UnityEngine;

public class FireDamageBuff : BuffEffect
{
    public GameObject FireEffect;
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        // yo!, replace particle here
    }
}

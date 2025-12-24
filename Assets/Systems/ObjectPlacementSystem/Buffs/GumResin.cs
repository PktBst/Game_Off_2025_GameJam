using UnityEngine;

public class GumResin : BuffEffect
{
    [SerializeField] private GameObject gumResinPrefab; 
    public override void Init()
    {
        if(TryGetComponent(out AttackComponent attackComponent))
        {
            attackComponent.projectileModel = gumResinPrefab;
            attackComponent.AdditionEffectOnTarget = OnAdditionalEffectOnTargetOnCollision;
        }
    }


    private void OnAdditionalEffectOnTargetOnCollision(Transform target)
    {
        if(target != null && target.TryGetComponent(out StatsComponent stats) && stats.FactionType!= GetComponent<StatsComponent>().FactionType)
        {
            stats.SpeedMultiplier = 0.5f;
            AnimationPool.Instance.Play_CFXR_Water_Splash_Smaller__AnimationAtFor(target.forward,target.position,0.6f);
        }
    }
    

}

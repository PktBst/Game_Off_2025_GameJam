using UnityEngine;

public class GumResin : BuffEffect
{
    public override void Init()
    {
        if (TryGetComponent(out AttackComponent attackComponent))
        {
            if (AnimationPool.Instance != null)
            {
                AnimationPool.Instance.Play_CFXR2_Shiny_Item_Loop__AnimationAtFor(transform.forward, transform.position, 0.7f);
            }
            attackComponent.AdditionEffectOnTarget += OnAdditionalEffectOnTargetOnCollision;
        }
    }
    private void OnDestroy()
    {
        if (TryGetComponent(out AttackComponent attackComponent))
        {
            attackComponent.AdditionEffectOnTarget -= OnAdditionalEffectOnTargetOnCollision;
        }
    }

    private void OnAdditionalEffectOnTargetOnCollision(Transform target)
    {
        if (target != null && target.TryGetComponent(out StatsComponent stats) && stats.FactionType != GetComponent<StatsComponent>().FactionType)
        {
            var hits = Physics.OverlapSphere(target.position, 1f);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out StatsComponent hitStats) && hitStats.FactionType != GetComponent<StatsComponent>().FactionType)
                {
                    hitStats.SpeedMultiplier = 0.5f;
                }
            }
            AnimationPool.Instance.Play_CFXR_Water_Splash_Smaller__AnimationAtFor(target.forward, target.position, 2f);
            Debug.Log("[Gum Resin]");
        }
    }


}

using System;
using UnityEngine;

public class TownHallAttackScript : MonoBehaviour
{
    Func<Vector3, Vector3, float, Vector3> lerpFunction;

    private void Awake()
    {
        GameManager.Instance.TickSystem.Subscribe(TickUpdate);
    }
    private void OnDestroy()
    {
        GameManager.Instance.TickSystem.Unsubscribe(TickUpdate);
    }
    public void SetLerpFunction(ProjectileBehavior projectileBehaviorScript)
    {
        lerpFunction = projectileBehaviorScript.LerpFunc;
    }
    public void TickUpdate()
    {
        var pos = GridSystem.Instance.CurrentTile;
        if (pos == null) return;
        if (Input.GetMouseButtonUp(1))
        {
            var projectile = ProjectilePool.Instance?.GetProjectile();
            projectile.Init(
                faction: Faction.GoodGuys,
                startPosition: transform.position,
                targetPosition: pos.Pos,
                damage: 3f,
                lerpFunc: lerpFunction,
                onTriggerEnterCallBack: OnTriggerEnterCall
            );
        }
    }
    public virtual void OnTriggerEnterCall(Collider other, Projectile projectile)
    {
        if (!other.TryGetComponent<HealthComponent>(out var targetHealth))
        {
            return;
        }

        if (targetHealth.Stats.FactionType != projectile.FactionType)
        {
            targetHealth.DeductHealth(projectile.Damage);
            projectile.Deactivate();
        }
    }
}

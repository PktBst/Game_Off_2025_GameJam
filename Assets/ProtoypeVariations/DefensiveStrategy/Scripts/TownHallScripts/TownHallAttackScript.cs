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
        if (GridSystem.Instance == null)
        {
            return;
        }
        Vector3? worldPos = null;
        if (DayNightCycleCounter.Instance != null && DayNightCycleCounter.Instance.CurrentTime == TimeOfDay.Day)
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, GridSystem.Instance.GroundLayer.value))
        {
            worldPos = hit.point;
        }
        if (!worldPos.HasValue) return;
        if (Input.GetMouseButtonUp(1))
        {
            var projectile = ProjectilePool.Instance?.GetProjectile();
            projectile.Init(
                faction: Faction.GoodGuys,
                startPosition: transform.position,
                targetPosition: worldPos.Value,
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

using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "AttackBehaviour_SO", menuName = "Scriptable Objects/AttackBehaviour_SO")]
public class PointTargetAttack_SO : AttackBehaviour_SO
{
    override public void ExecuteBehaviour(AttackComponent attackComponent, Transform AttackPoint, Transform Target)
    {
        
        if (ProjectilePool.Instance != null)
        {
            var projectile = ProjectilePool.Instance.GetProjectile();
            projectile.Init(Faction.GoodGuys, AttackPoint.position, Target.position, damage: BaseDamage, lerpFunc: ProjectileBehaviorScript.LerpFunc,onTriggerEnterCallBack:OnTriggerEnterCall, firedFrom: attackComponent,attackComponent.projectileModel);
            Debug.Log(name + "<color=purple> Executed Point Attack Behaviour !</color>");
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
            if(projectile.FiredFromAttackComponent!=null && projectile.FiredFromAttackComponent.AdditionEffectOnTarget != null)
            {
                projectile.FiredFromAttackComponent.AdditionEffectOnTarget(targetHealth.transform);
            }
        }
    }
}

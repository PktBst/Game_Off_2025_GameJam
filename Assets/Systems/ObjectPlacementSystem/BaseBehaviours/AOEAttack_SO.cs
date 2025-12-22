using UnityEngine;

[CreateAssetMenu(fileName = "AOEAttack_SO", menuName = "Scriptable Objects/AOEAttack_SO")]
public class AOEAttack_SO : AttackBehaviour_SO
{
    override public void ExecuteBehaviour(AttackComponent attackComponent, Transform AttackPoint, Transform Target)
    {
        Debug.Log(name + " Executed Base Behaviour !");

        if (ProjectilePool.Instance != null)
        {
            var projectile = ProjectilePool.Instance.GetProjectile();
            projectile.Init(Faction.GoodGuys, AttackPoint.position, Target.position, damage: BaseDamage, lerpFunc: ProjectileBehaviorScript.LerpFunc, onTriggerEnterCallBack: OnTriggerEnterCall, model: attackComponent.projectileModel);
            Debug.Log(name + "<color=purple> Executed Point Attack Behaviour !</color>");
        }
    }

    public virtual void OnTriggerEnterCall(Collider other, Projectile projectile)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Platform"))
        {
            return;
        }
        projectile.Deactivate();
        float radius = 3f;
        Collider[] hits = Physics.OverlapSphere(projectile.transform.position, radius);

        foreach (var hit in hits)
        {
            // Check if the collider has a HealthComponent
            if (hit.TryGetComponent<HealthComponent>(out var targetHealth))
            {
                // Only damage if factions are different
                if (targetHealth.Stats.FactionType != projectile.FactionType)
                {
                    targetHealth.DeductHealth(projectile.Damage);
                }
            }
        }
    }
}

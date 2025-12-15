using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "AttackBehaviour_SO", menuName = "Scriptable Objects/AttackBehaviour_SO")]
public class PointTargetAttack_SO : AttackBehaviour_SO
{
    override public void ExecuteBehaviour(Transform AttackPoint, Transform Target)
    {
        
        if (ProjectilePool.Instance != null)
        {
            var projectile = ProjectilePool.Instance.GetProjectile();
            projectile.Init(Faction.GoodGuys, AttackPoint.position, Target.position, damage: 1f);
            Debug.Log(name + "<color=purple> Executed Point Attack Behaviour !</color>");
        }
    }
}

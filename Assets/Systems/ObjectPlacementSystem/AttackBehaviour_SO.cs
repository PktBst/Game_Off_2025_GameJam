using UnityEngine;

public class AttackBehaviour_SO : BaseBehaviour_SO
{
    public GameObject ProjectilePrefab;
    public GameObject HitParticleEffect;
    public float BaseDamage;
    public float AttackCooldown;
    public ProjectileBehavior ProjectileBehaviorScript;

    override public void ExecuteBehaviour(Transform AttackPoint, Transform Target)
    {
        Debug.Log(name + " Executed Base Behaviour !");
    }
}

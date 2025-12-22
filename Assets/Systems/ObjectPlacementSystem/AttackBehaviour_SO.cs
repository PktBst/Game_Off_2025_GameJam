using UnityEngine;

public class AttackBehaviour_SO : BaseBehaviour_SO
{
    public GameObject HitParticleEffect;
    public float BaseDamage;
    public float AttackCooldown;
    public ProjectileBehavior ProjectileBehaviorScript;
    public GameObject projectileModel;
    override public void ExecuteBehaviour(AttackComponent attackComponent ,Transform AttackPoint, Transform Target)
    {
        Debug.Log(name + " Executed Base Behaviour !");
    }
}

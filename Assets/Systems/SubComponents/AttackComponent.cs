using System.Collections;
using UnityEngine;


[RequireComponent(typeof(StatsComponent))]
public class AttackComponent : MonoBehaviour
{
    public StatsComponent stats;
    public Animator animator;

    public Coroutine animationCoroutine;

    public StatsComponent Stats
    {
        get
        {
            stats ??= GetComponent<StatsComponent>();
            return stats;
        }
    }
    public HealthComponent targetHealth;


    private void Start()
    {
        GameManager.Instance.TickSystem.Subscribe(UpdateTarget);
    }
    private void OnDestroy()
    {
        GameManager.Instance.TickSystem.Unsubscribe(UpdateTarget);
    }

    private void UpdateTarget()
    {
        if(ScanForTarget(out targetHealth))
        {
            animationCoroutine ??= StartCoroutine(playAttackAnimation());   
        }
    }
    IEnumerator playAttackAnimation()
    {
        animator.SetBool("IsAttacking",true);
        yield return new WaitForSeconds(stats.BaseAttackSpeed);
        targetHealth?.DeductHealth(Stats.BaseAttackPoints);
        animator.SetBool("IsAttacking", false);
        animationCoroutine = null;
    }
    public bool ScanForTarget(out HealthComponent targetHealth)
    {
        var hits = Physics.OverlapSphere(transform.position, 1f);

        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent<HealthComponent>(out var health))
            {
                continue;
            }
            if (health.Stats.FactionType != Stats.FactionType)
            {
                targetHealth = health;
                return true;
            }

        }
        targetHealth = null;
        return false;
    }

}

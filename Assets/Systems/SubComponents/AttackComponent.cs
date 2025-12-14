using System.Collections;
using UnityEngine;


[RequireComponent(typeof(StatsComponent))]
public class AttackComponent : MonoBehaviour
{
    public StatsComponent stats;
    public Animator animator;

    public Coroutine animationCoroutine;

    public Transform projectileSpawnPoint;
    public bool IsRanged;
    const float duration = 5;
    float elapsed = duration;

    public bool hasTarget => targetHealth != null;
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
            if (IsRanged)
            {
                if (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    return;
                }
                elapsed = 0;
                if (ProjectilePool.Instance != null)
                {
                    var projectile = ProjectilePool.Instance.GetProjectile();
                    projectile.Init(Stats.FactionType,projectileSpawnPoint.transform.position,targetHealth.transform.position, Stats.BaseAttackPoints, Stats.BaseAttackSpeed);
                    projectile.Activate();
                }
            }
            else
            {
                animationCoroutine ??= StartCoroutine(playAttackAnimation());   
            }
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
        float scanRadius = IsRanged ? 5f : 1f;
        var hits = Physics.OverlapSphere(transform.position, scanRadius);

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

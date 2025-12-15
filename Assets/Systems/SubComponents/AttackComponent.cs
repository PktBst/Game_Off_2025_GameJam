using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[RequireComponent(typeof(StatsComponent))]
public class AttackComponent : MonoBehaviour
{
    public StatsComponent stats;
    public Animator animator;

    public Coroutine animationCoroutine;

    public Transform projectileSpawnPoint;
    public bool IsRanged;
    const float duration = 3;
    float elapsed = duration;
    float detectionRadius => IsRanged ? 3f : 0.5f;

    float scanRadius => Stats?.FactionType == Faction.GoodGuys? 5 :15;

    public bool hasTarget => targetHealth != null;

    private bool stopScanning= false;

    public StatsComponent Stats
    {
        get
        {
            stats ??= GetComponent<StatsComponent>();
            return stats;
        }
    }

    private MoveComponent move;

    public MoveComponent Move
    {
        get
        {
            move ??= GetComponent<MoveComponent>();
            return move;
        }
    }

    public HealthComponent targetHealth;
    bool wasStopped = true;

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
        if(ScanForNearestTarget(out targetHealth))
        {
            if ((targetHealth.transform.position-transform.position).sqrMagnitude <= detectionRadius * detectionRadius)
            {
                if(!wasStopped)
                {
                    Move?.Stop();
                    Move?.MoveTo(transform.position);
                    Move?.Resume();
                }
                AttackOnTarget();
                wasStopped = true;
            }
            else
            {
                wasStopped = false;
                Move?.MoveTo(targetHealth.transform.position); 
            }
        }

    }
    void AttackOnTarget()
    {
        if (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            return;
        }
        elapsed = 0;
        if (IsRanged)
        {
            animationCoroutine ??= StartCoroutine(PlayRangedAttack());
        }
        else
        {
            animationCoroutine ??= StartCoroutine(playAttackAnimation());
        }
    }

    IEnumerator PlayRangedAttack()
    {
        while(elapsed<duration && targetHealth != null)
        {
            if (ProjectilePool.Instance != null)
            {
                var projectile = ProjectilePool.Instance.GetProjectile();
                projectile.Init(Stats.FactionType, projectileSpawnPoint.position, targetHealth.transform.position, Stats.BaseAttackPoints);
            }
            yield return new WaitForSeconds(1f);
        }
        animationCoroutine = null;
    }


    IEnumerator playAttackAnimation()
    {
        animator?.SetBool("IsAttacking",true);
        while (elapsed < duration && targetHealth !=null)
        {
            targetHealth?.DeductHealth(Stats.BaseAttackPoints);
            yield return new WaitForSeconds(1f);
        }
        animator?.SetBool("IsAttacking", false);
        animationCoroutine = null;
    }
    public void ResumeScanning()
    {
        stopScanning = false;
    }
    public void StopScanning()
    {
        stopScanning = true;
    }
    bool ScanForNearestTarget(out HealthComponent targetHealth)
    {
        targetHealth = null;

        if (Move!=null && Move.DestinationReached)
        {
            ResumeScanning();
        }

        if (stopScanning)
        {
            return false;
        }
        Collider[] hits = Physics.OverlapSphere(transform.position, scanRadius);

        float closestDistanceSqr = float.MaxValue;
        HealthComponent nearestTarget = null;

        Vector3 myPos = transform.position;

        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent<HealthComponent>(out var health))
                continue;

            if (health.gameObject == gameObject)
                continue;

            if (health.Stats.FactionType == Stats.FactionType)
                continue;

            float distSqr = (health.transform.position - myPos).sqrMagnitude;

            if (distSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distSqr;
                nearestTarget = health;
            }
        }

        targetHealth = nearestTarget;
        return nearestTarget != null;
    }

}

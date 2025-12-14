using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveComponent : MonoBehaviour
{
    public float MoveSpeed;
    public Vector3 TargetPosition;
    public bool IsMoving => !Agent.isStopped;

    private NavMeshAgent agent;

    public NavMeshAgent Agent
    {
        get
        {
            agent ??= GetComponent<NavMeshAgent>();
            return agent;
        }
    }

    public void MoveTo(Vector3 targetPosition)
    {
        TargetPosition = targetPosition;
        Agent.SetDestination(targetPosition);
        Agent.speed = MoveSpeed;
    }
    bool ScanForTarget(out HealthComponent targetHealth)
    {
        targetHealth = null;

        if (!TryGetComponent<StatsComponent>(out var myStats))
            return false;

        Collider[] hits = Physics.OverlapSphere(transform.position, 15f);

        float closestDistanceSqr = float.MaxValue;
        HealthComponent nearestTarget = null;

        Vector3 myPos = transform.position;

        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent<HealthComponent>(out var health))
                continue;

            if (health.gameObject == gameObject)
                continue;

            if (health.Stats.FactionType == myStats.FactionType)
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


    void UpdateTarget()
    {

        //Vector3 dir = (TargetPosition - transform.position).normalized;
        transform.LookAt(TargetPosition);
        if (ScanForTarget(out var targetHealth))
        {
            MoveTo(targetHealth.transform.position);
        }
    }

    private void Start()
    {
        GameManager.Instance.TickSystem.Subscribe(UpdateTarget);
    }
    private void OnDestroy()
    {
        GameManager.Instance.TickSystem.Unsubscribe(UpdateTarget);
    }
}

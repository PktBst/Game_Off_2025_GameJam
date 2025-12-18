using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveComponent : MonoBehaviour
{
    public float MoveSpeed;
    public Vector3 TargetPosition;
    public bool IsMoving => !Agent.isStopped;
    public bool DestinationReached => getDistance();
    private NavMeshAgent agent;

    bool getDistance() {
        if (this == null || Agent == null || !Agent.isActiveAndEnabled) return false;
        return Agent?.remainingDistance <= 0.5f;
    }
    public NavMeshAgent Agent
    {
        get
        {
            agent ??= GetComponent<NavMeshAgent>();
            return agent;
        }
    }
    private StatsComponent stats;
    public StatsComponent Stats
    {
        get
        {
            stats ??= GetComponent<StatsComponent>();
            return stats;
        }
    }
    public void MoveTo(Vector3 targetPosition)
    {
        if(Agent == null) return;
        TargetPosition = targetPosition;
        Agent?.SetDestination(targetPosition);
        Agent.speed = MoveSpeed;
    }
    public void Stop()
    {
        Agent.isStopped = true;
    }

    public void Resume()
    {
        Agent.isStopped = false;
    }


    void UpdateTarget()
    {
        if (this == null || Agent == null || !Agent.isActiveAndEnabled) return;
        if (TryGetComponent<AttackComponent>(out var attack))
        {
            if (attack.hasTarget)
            {
                LookAtFlat(attack.targetHealth.transform.position);
                return;
            }
        }
        Resume();
        LookAtFlat(TargetPosition);
    }
    void LookAtFlat(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0f; // prevent tilt

        if (direction.sqrMagnitude < 0.001f)
            return;

        transform.rotation = Quaternion.LookRotation(direction);
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

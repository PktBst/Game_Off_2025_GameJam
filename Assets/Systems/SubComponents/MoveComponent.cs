using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveComponent : MonoBehaviour
{
    public bool Moveable;
    public float MoveSpeed;
    public Vector3 TargetPosition;

    public NavMeshAgent Agent;

    public void MoveTo(Vector3 targetPosition)
    {
        TargetPosition = targetPosition;
        Agent ??= GetComponent<NavMeshAgent>();
        Agent.Move(targetPosition);
    }
    
}

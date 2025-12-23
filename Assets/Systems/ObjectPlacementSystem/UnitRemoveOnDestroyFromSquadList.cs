using UnityEngine;
using UnityEngine.AI;

public class UnitRemoveOnDestroyFromSquadList : MonoBehaviour
{
    private NavMeshAgent NavMeshAgent;
    private void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnDestroy()
    {
        transform.parent.GetComponent<SquadPost>().UnitList.Remove(NavMeshAgent);
    }
}

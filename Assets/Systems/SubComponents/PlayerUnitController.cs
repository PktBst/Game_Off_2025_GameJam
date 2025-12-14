using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(MoveComponent))]
public class PlayerUnitController : MonoBehaviour
{
    MoveComponent moveComponent;
    void Update()
    {
        moveComponent ??= GetComponent<MoveComponent>();
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                moveComponent.MoveTo(hit.point);
                Debug.Log($"Agent Moved To {hit.point}");
            }
        }
    }
}

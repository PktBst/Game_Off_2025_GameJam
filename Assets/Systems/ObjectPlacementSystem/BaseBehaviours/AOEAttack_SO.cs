using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "AOEAttack_SO", menuName = "Scriptable Objects/AOEAttack_SO")]
public class AOEAttack_SO : AttackBehaviour_SO
{
    override public void ExecuteBehaviour(Transform AttackPoint, Transform Target)
    {
        Debug.Log(name + " Executed Base Behaviour !");
    }
}

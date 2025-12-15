using UnityEngine;

[CreateAssetMenu(fileName = "BaseBehaviour_SO", menuName = "Scriptable Objects/BaseBehaviour_SO")]
public class BaseBehaviour_SO : ScriptableObject    
{
    public new string name;
    virtual public void ExecuteBehaviour(Transform AttackPoint, Transform Target)
    {
        Debug.Log(name + " Executed Base Behaviour !");
    }
}

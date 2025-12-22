using UnityEngine;

[CreateAssetMenu(fileName = "BaseBehaviour_SO", menuName = "Scriptable Objects/BaseBehaviour_SO")]
public class BaseBehaviour_SO : ScriptableObject    
{
    public new string name;
    virtual public void ExecuteBehaviour(AttackComponent attackComponent,Transform AttackPoint, Transform Target)
    {
        Debug.Log(name + " Executed Base Behaviour !");
    }
}

using UnityEngine;

public class MiscEffect : MonoBehaviour
{
    public virtual bool TryUsingThisCard()
    {
        Debug.Log("Used Misc Effect card.");
        return true;
    }

    public virtual void Init()
    {
        Debug.Log("Base Misc Init was called");
    }
}

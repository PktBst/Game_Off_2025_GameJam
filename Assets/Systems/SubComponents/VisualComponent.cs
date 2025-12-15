using UnityEngine;

public class VisualComponent : MonoBehaviour
{
    public Transform ModelHolder;

    public void Init(GameObject GameModel)
    {
        CleanModelHolder();
        Instantiate(GameModel,ModelHolder);
    }

    public void CleanModelHolder()
    {
        //foreach (Transform t in ModelHolder.transform) { Destroy(t.gameObject); }
    }
}

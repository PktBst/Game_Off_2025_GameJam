using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.rotation = cam.rotation;
    }
}

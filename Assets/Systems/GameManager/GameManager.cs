using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] InputActionAsset _inputSystem;
    [SerializeField] GridSystem _gridSystem;

    private void Start()
    {
        Init();
    }

    //Initializes All Other Managers 
    void Init()
    {
        InitInputSystem();
        _gridSystem.Init();
    }

    void InitInputSystem()
    {

    }
}


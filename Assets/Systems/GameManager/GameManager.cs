using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] InputActionAsset _inputSystem;
    [SerializeField] GridSystem _gridSystem;
    [SerializeField] ObjectPlacementSystem _objectPlacementSystem;

    public static GameManager Instance;

    public InputActionAsset InputActionAsset => _inputSystem;
    public ObjectPlacementSystem ObjectPlacementSystem => _objectPlacementSystem;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
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


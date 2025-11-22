using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] InputActionAsset _inputSystem;
    [SerializeField] TickSystem _tickSystem;
    [SerializeField] GridSystem _gridSystem;
    [SerializeField] ObjectPlacementSystem _objectPlacementSystem;
    [SerializeField] CardSystem _cardSystem;

    public static GameManager Instance;

    public TickSystem TickSystem => _tickSystem;
    public GridSystem GridSystem => _gridSystem;
    public InputActionAsset InputActionAsset => _inputSystem;
    public ObjectPlacementSystem ObjectPlacementSystem => _objectPlacementSystem;
    public CardSystem CardSystem => _cardSystem;
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
        _tickSystem.Init();
        InitInputSystem();
        _gridSystem.Init();
        _cardSystem.Init();
    }

    void InitInputSystem()
    {

    }
}


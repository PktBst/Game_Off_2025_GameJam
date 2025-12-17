using AYellowpaper.SerializedCollections;
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
    public GameMode CurrentGameMode;

    [SerializedDictionary("Game Mode Type", "Game Mode Script")]
    public SerializedDictionary<GameModeType,GameMode> GameModesList;
    [SerializeField] public GameModeType PlayThisGameMode;

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

        //stuff
        CurrentGameMode = Instantiate(GameModesList[PlayThisGameMode].gameObject,transform).GetComponent<GameMode>();
        CurrentGameMode.Init();
    }

    void InitInputSystem()
    {

    }

}




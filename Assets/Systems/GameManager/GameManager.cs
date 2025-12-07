using System.Collections.Generic;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.InputSystem;
using static AYellowpaper.SerializedCollections.SerializedDictionarySample;

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

    [SerializedDictionary("Game Mode Type", "Game Mode Script")]
    public SerializedDictionary<GameModeType,GameMode> GameModesList;

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
        GameModesList[GameModeType.InfiniteWaves].Init();
    }

    void InitInputSystem()
    {

    }

}




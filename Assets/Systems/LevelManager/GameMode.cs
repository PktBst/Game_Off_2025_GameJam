using UnityEngine;

[System.Serializable]
public class GameMode : MonoBehaviour
{
    public virtual void Init() { }
}
[System.Serializable]
public enum TimeOfDay
{
    Day = 0,
    Night
}
[System.Serializable]
public enum GameModeType
{
    InfiniteWaves,
    DefensiveStrategy,
    TurnBased
}
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameMode : MonoBehaviour
{
    public List<GameObject> EnemyList;
    public virtual void Init() { }

    public bool CheckIfEnemyLeft() { return EnemyList.Count > 0; }
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
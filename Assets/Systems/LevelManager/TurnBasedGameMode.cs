using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedGameMode : GameMode
{
    [Header("Setup")]
    [SerializeField] GameObject TownHallPrefab;
    [SerializeField] List<GameObject> SpawnableEnemyPrefabList;
    private GameObject _townHall;
    public PlayTurn CurrentTurn { get; private set; }
    public event Action<PlayTurn> OnTurnBegun;

    public override void Init()
    {
        Tile originTile = GridSystem.GetTileByCoordinates(0, 0);
        _townHall = Instantiate(TownHallPrefab, originTile.Pos, Quaternion.identity);
        originTile.TryOccupyTileEntity(_townHall);
        CurrentTurn = PlayTurn.Player;
        BeginTurn();
    }

    public void EndCurrentTurn()
    {
        CurrentTurn = (CurrentTurn == PlayTurn.Player)? PlayTurn.CPU: PlayTurn.Player;
        if (CurrentTurn == PlayTurn.Player)
        {
            DayNightCycleCounter.Instance.SetDay();
        }
        else
        {
            DayNightCycleCounter.Instance.SetNight();
        }

        BeginTurn();
    }

    private void BeginTurn()
    {
        GameManager.Instance.CardSystem.ToggleAndMoveDeck();
        Debug.Log($"Turn Begun: {CurrentTurn}");
        OnTurnBegun?.Invoke(CurrentTurn);
        if (CurrentTurn == PlayTurn.CPU)
        {
            int randomIndex = UnityEngine.Random.Range(0, SpawnableEnemyPrefabList.Count);
            Vector3 pos = Vector3.one * -5;
            pos.y = 0;
            pos.x *= -1;
            GameObject gm = Instantiate(SpawnableEnemyPrefabList[randomIndex],pos,Quaternion.identity);
            EnemyList.Add(gm);
            gm.GetComponent<HealthComponent>().OnDeath += CheckOnEnemyUnitDeath;
            Debug.Log($"CPU Used its Turn");
        }
    }

    public void CheckOnEnemyUnitDeath()
    {
        if (base.CheckIfEnemyLeft()) EndCurrentTurn();
    }


    public enum PlayTurn
    {
        Player,
        CPU
    }
}
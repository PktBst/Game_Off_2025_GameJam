using UnityEngine;

public class DefensiveStrategyGameMode : GameMode
{
    [SerializeField] GameObject TownHallPrefab;
    private GameObject _townHall;
    public ProjectileBehavior ProjectileBehaviorScript;
    public override void Init()
    {
        Tile originTile = GridSystem.GetTileByCoordinates(0, 0);
        _townHall = Instantiate(TownHallPrefab);
        _townHall.transform.position = originTile.Pos;
        originTile.TryOccupyTileEntity(_townHall);
        _townHall.AddComponent<TownHallAttackScript>().SetLerpFunction(projectileBehaviorScript:  ProjectileBehaviorScript);
    }
}

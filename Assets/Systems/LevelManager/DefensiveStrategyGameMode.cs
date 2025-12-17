using UnityEngine;

public class DefensiveStrategyGameMode : GameMode
{
    [SerializeField] GameObject TownHallPrefab;
    private GameObject _townHall;

    public override void Init()
    {
        Tile originTile = GridSystem.GetTileByCoordinates(0, 0);
        _townHall = Instantiate(TownHallPrefab);
        _townHall.transform.position = originTile.Pos;
        originTile.TryOccupyTileEntity(_townHall);
    }
}

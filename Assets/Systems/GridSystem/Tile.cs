
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public int _x;
    public int _z;

    public int X => _x;
    public int Z => _z;

    public float G_Cost = 0f;
    public float H_Cost = 0f;
    public float F_Cost => G_Cost + H_Cost;

    public GameObject OccupyingEntity;
    public GameObject OccupyingUnit;
    public List<Objective> Objectives;

    public bool IsBlocked => OccupyingEntity != null;

    public Vector3Int Pos => new(_x, 0, _z);

    public bool TryOccupyTileUnit(GameObject unit)
    {
        if (OccupyingEntity != null)
        {
            return false;
        }
        if (OccupyingUnit != null && OccupyingUnit == unit)
        {
            return true;
        }
        if (OccupyingUnit != null)
        {
            return false;
        }

        OccupyingUnit = unit;
        return true;
    }
    public bool TryOccupyTileEntity(GameObject entity)
    {
        if (OccupyingEntity != null || OccupyingUnit != null)
        {
            return false;
        }
        OccupyingEntity = entity;
        return true;
    }
    public Tile(int x, int z)
    {
        this._x = x;
        this._z = z;
    }
}

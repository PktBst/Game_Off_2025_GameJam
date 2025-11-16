using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class PathFinding
{
    private readonly List<Tile> _openList = new();
    private readonly List<Tile> _closeList = new();

    private float Heuristic(Tile startTile, Tile endTile)
    {
        return Mathf.Abs(startTile._x - endTile._x)+ Mathf.Abs(startTile._z - endTile._z);
    }

    private float CalculateDistance(Tile startTile, Tile endTile)
    {
        return Vector3.Distance(startTile.Pos, endTile.Pos);
    }

    public List<Tile> FindPath(Tile startTile,Tile endTile)
    {
        _openList.Clear();
        _closeList.Clear();
        startTile.G_Cost = 0;
        startTile.H_Cost = Heuristic(startTile, endTile);
        _openList.Add(startTile);
        while(_openList.Count > 0)
        {
            _openList.Sort((a,b)=>a.F_Cost.CompareTo(b.F_Cost));
            Tile current = _openList[0];
            _openList.RemoveAt(0);
            if (current == endTile) 
            {
                List<Tile> path = new ();
                Tile currentTile = current;
                while (currentTile != startTile)
                {
                    path.Add(currentTile);
                    foreach(Tile nTile in GetNeighbors(current))
                    {
                        if(!nTile.IsBlocked && _closeList.Contains(nTile) && nTile.G_Cost == currentTile.G_Cost-1)
                        {
                            currentTile = nTile;
                            break;
                        }
                    }
                }
                path.Add(startTile);
                path.Reverse();
                return path;
            }
            _closeList.Add(current);
            foreach(Tile nTile in GetNeighbors(current))
            {
                if (!_closeList.Contains(nTile))
                {
                    float tentativeG_Cost = current.G_Cost + 1;
                    if(!_openList.Contains(nTile) || tentativeG_Cost < nTile.G_Cost)
                    {
                        nTile.G_Cost = tentativeG_Cost;
                        nTile.H_Cost = Heuristic(nTile,endTile);
                        if (!_openList.Contains(nTile))
                        {
                            _openList.Add(nTile);
                        }
                    }
                }

            }
        }

        return null;
    }

    private List<Tile> GetNeighbors(Tile tile)
    {
        Vector3 pos = tile.Pos;
        List<Tile> neighbors = new ();
        List<Vector3> directions = new ()
        {
            new (1,0,0),
            new (-1,0,0),
            new (0,0,1),
            new (0,0,-1)
        };

        foreach (var direction in directions)
        {
            Vector3 neighbor = pos + direction;
            if (GridSystem.TryGetTileByWorldPosition(neighbor,out Tile nTile) && !nTile.IsBlocked)
            {
                neighbors.Add(nTile);
            }
        }
        return neighbors;
    }

}

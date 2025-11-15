using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] GameObject GridPlane;
    [SerializeField] Vector2 GridDimensions;
    public static GridSystem Instance;
    [HideInInspector] public Dictionary<Vector2,Tile> AllTiles = new();
    

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

    public void Init()
    {
        for (int i = 0; i < GridDimensions.x; i++)
        {
            for (int j = 0; j < GridDimensions.y; j++)
            {
                AllTiles.Add(new Vector2(i,j),new Tile(i,j));
            }
        }
    }

    public static Tile GetTileByWorldPosition(Vector3 WorldPosition)
    {
        return Instance.AllTiles[new Vector2(Mathf.Ceil(WorldPosition.x), Mathf.Ceil(WorldPosition.z))];
    }

    public static Tile GetTileByCoordinates(int X_Coordinate,int Z_Coordinate)
    {
        return Instance.AllTiles[new Vector2(X_Coordinate, Z_Coordinate)];
    }

    #region Debug - Refactor Later
    int tileSize = 1;
    int height = 1;
    int width = 1;
    void OnDrawGizmos()
    {
        if (AllTiles == null) return;

        Gizmos.color = Color.gray;

        // Draw grid lines
        for (int x = 0; x <= width; x++)
        {
            Gizmos.DrawLine(new Vector3(x * tileSize, 0, 0), new Vector3(x * tileSize, 0, height * tileSize));
        }
        for (int y = 0; y <= height; y++)
        {
            Gizmos.DrawLine(new Vector3(0, 0, y * tileSize), new Vector3(width * tileSize, 0, y * tileSize));
        }

        // Draw each tile
        foreach (var tile in AllTiles)
        {
            Vector2 tilePosition = tile.Key;
            Tile tileData = tile.Value;

            // Use the tile's position and color
            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector3(tilePosition.x, 0.1f, tilePosition.y), new Vector3(tileSize, 0.1f, tileSize));
        }
    }
    #endregion
}


public class Tile
{
    public int x;
    public int z;
    public GameObject OccupyingEntity;

    public bool isBlocked => OccupyingEntity != null;

    public Tile(int x, int z)
    {
        this.x=x;
        this.z=z;
    }
}

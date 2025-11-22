using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridSystem : MonoBehaviour
{
    private GameObject _gridPlane;
    [SerializeField] Vector2 GridDimensions;
    [SerializeField] bool showDebugInfo;
    [SerializeField] TextMeshProUGUI DebugCurrentTileText;
    public static GridSystem Instance;
    [HideInInspector] public Dictionary<Vector2, Tile> AllTiles = new();

    public Tile CurrentTile
    {
        get
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())  return null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject == _gridPlane)
                {
                    return GetTileByWorldPosition(hit.point);
                }
            }
            return null;
        }
    }

    private void Update()
    {
        if (showDebugInfo) updateDebugInfo();
        if (Input.GetMouseButtonDown(0))
        {
            if (CurrentTile != null)
            {
                Debug.Log($"Tile: <b>{CurrentTile._x}, {CurrentTile._z}</b>");
            }
            else
            {
                Debug.Log($"Tile: <b>NULL</b>");
            }
                

        }
    }
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
                AllTiles.Add(new Vector2(i, j), new Tile(i, j));
            }
        }
        _gridPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        _gridPlane.transform.SetParent(transform, false);
        _gridPlane.transform.localScale = new(GridDimensions.x / 10f, 1, GridDimensions.y / 10f);
        _gridPlane.transform.position += new Vector3(_gridPlane.transform.localScale.x,0, _gridPlane.transform.localScale.z)*(4.5f);
    }

    public static Tile GetTileByWorldPosition(Vector3 WorldPosition)
    {
        return GetTileByCoordinates(Mathf.CeilToInt(WorldPosition.x), Mathf.CeilToInt(WorldPosition.z));
    }

    public static bool TryGetTileByWorldPosition(Vector3 WorldPosition,out Tile tile)
    {
        tile = GetTileByCoordinates(Mathf.CeilToInt(WorldPosition.x), Mathf.CeilToInt(WorldPosition.z));
        return tile != null;
    }

    public static Tile GetTileByCoordinates(int X_Coordinate, int Z_Coordinate)
    {
        if(Instance == null || Instance.AllTiles==null || !Instance.AllTiles.ContainsKey(new Vector2(X_Coordinate, Z_Coordinate)))
        {
            return null;
        }
        return Instance.AllTiles[new Vector2(X_Coordinate, Z_Coordinate)];
    }

    #region Debug - Refactor Later
    readonly int tileSize = 1;
    readonly int height = 1;
    readonly int width = 1;
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

            // Use the tile's position and color
            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector3(tilePosition.x, 0.1f, tilePosition.y), new Vector3(tileSize, 0.1f, tileSize));
        }
    }

    void updateDebugInfo()
    {
        if (CurrentTile == null)
        {
            DebugCurrentTileText.text = "Position: NULL";
            return;
        }

        string entityName = CurrentTile.OccupyingEntity != null ? CurrentTile.OccupyingEntity.name : "NULL";
        string unitName = CurrentTile.OccupyingUnit != null ? CurrentTile.OccupyingUnit.name : "NULL";

        DebugCurrentTileText.text =
            $"Position : ({CurrentTile.X},{CurrentTile.Z})  \n " +
            $"IsBlocked : {CurrentTile.IsBlocked}\n" +
            $"OccupyingEntity : {entityName} \n" +
            $"OccupyingUnit : {unitName} \n".ToString();
    }

    #endregion
}


public class Tile
{
    public int _x;
    public int _z;

    public int X => _x;
    public int Z => _z;

    public float G_Cost = 0f;
    public float H_Cost = 0f;
    public float F_Cost => G_Cost+H_Cost;

    public GameObject OccupyingEntity;
    public GameObject OccupyingUnit;

    public bool IsBlocked => OccupyingEntity != null;

    public Vector3Int Pos => new (_x, 0 ,_z);

    public bool TryOccupyTileUnit(GameObject unit)
    {
        if(OccupyingEntity != null)
        {
            return false;
        }
        if(OccupyingUnit!=null &&  OccupyingUnit == unit)
        {
            return true;
        }
        if(OccupyingUnit != null)
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

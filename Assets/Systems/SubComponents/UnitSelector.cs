using UnityEngine;
using System.Collections.Generic;
using UnityEditor.UIElements;

public class UnitSelector : MonoBehaviour
{
    Camera Cam;
    List<StatsComponent> selectedUnits = new();
    public List<StatsComponent> unitList;

    [SerializeField] LayerMask layer;

    Rect selectionBox;
    Vector3 startPosition;
    Vector3 endPosition;
    public RectTransform boxVisual;


    float dragThreshold = 2f;

    Vector3 mouseDownPos;
    bool isDragging;
    private void Start()
    {
        Cam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual();
        selectionBox = new Rect();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Input.mousePosition;
            startPosition = mouseDownPos;
            isDragging = false;
        }

        if (Input.GetMouseButton(0))
        {
            endPosition = Input.mousePosition;

            if ((endPosition - mouseDownPos).sqrMagnitude > dragThreshold * dragThreshold)
            {
                isDragging = true;
                DrawVisual();
                DrawSelection();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                // DRAG SELECT
                SelectUnits();
            }
            else
            {
                // CLICK MOVE
                moveSelectedUnits();
            }

            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            selectionBox = new Rect();
            DrawVisual();
            isDragging = false;
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            DeSelectAll();
        }
    }

    void DrawVisual()
    {
        Vector2 boxstart = startPosition;
        Vector2 boxend = endPosition;

        Vector2 boxCenter = (boxstart + boxend) / 2;
        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxstart.x - boxend.x), Mathf.Abs(boxstart.y - boxend.y));

        boxVisual.sizeDelta = boxSize;
    }
    void DrawSelection()
    {
        // do X calculations
        if (Input.mousePosition.x < startPosition.x)
        {
            //dragging left
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPosition.x;
        }
        else
        {
            //dragging right
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }
        // do Y calculations
        if (Input.mousePosition.y < startPosition.y)
        {
            // dragging down
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPosition.y;
        }
        else
        {
            // dragging up
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }

    }

    void SelectUnits()
    {
        DeSelectAll();
        for(int i=0;i<unitList.Count;i++)
        {
            var unit = unitList[i];
            if (unit ==  null)
            {
                unitList.RemoveAt(i);
                continue;
            }
            if (selectionBox.Contains(Cam.WorldToScreenPoint(unit.transform.position)))
            {
                DragSelect(unit);
            }
        }
    }

    public void DragSelect(StatsComponent unitToAdd)
    {
        if (!selectedUnits.Contains(unitToAdd))
        {
            selectedUnits.Add(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    public void DeSelectAll()
    {
        foreach (var unitToRemove in selectedUnits)
        {
            if(unitToRemove == null) continue;
            unitToRemove.transform.GetChild(0).gameObject.SetActive(false);
        }
        selectedUnits.Clear();
    }

    void moveSelectedUnits()
    {
        Vector3? worldPos = null;
        Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity,layer))
        {
            worldPos = hit.point;
        }
        if(!worldPos.HasValue) return;

        foreach (var unit in selectedUnits)
        {
            if (unit.TryGetComponent(out AttackComponent attack))
            {
                attack.StopScanning();
            }
            if (unit.TryGetComponent(out MoveComponent moveComponent))
            {
                moveComponent.MoveTo(worldPos.Value);
            }
        }

    }
}

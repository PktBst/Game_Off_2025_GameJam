using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class SquadPost : MonoBehaviour
{
    [Header("Targets")]
    public Transform SquadPostPiller;
    public List<NavMeshAgent> UnitList = new List<NavMeshAgent>();

    [Header("Settings")]
    public float followTriggerRange = 5.0f;
    public float unitSpacing = 2.0f;

    [Header("Formation")]
    public FormationType currentFormation = FormationType.Box;
    public int boxColumns = 3;

    public enum FormationType { Line, Box }

    // Internal State
    private Vector3 _squadAnchorPosition;
    private bool _isDragging = false;
    private Camera _cam;
    private LineRenderer _lineRenderer;
    private Vector3 _dragOffset;

    // Circle Settings
    private int _circleSegments = 50;

    private void Start()
    {
        _cam = Camera.main;
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = _circleSegments;
        _lineRenderer.loop = true;
        _lineRenderer.useWorldSpace = true;

        if (SquadPostPiller != null)
            _squadAnchorPosition = SquadPostPiller.position;
    }

    private void Update()
    {
        if (SquadPostPiller == null) return;

        HandleInput();
        //CheckMovementTrigger();
        UpdateRangeCircle();
    }

    // --- 1. Click and Drag Logic ---
    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == SquadPostPiller)
                {
                    _isDragging = true;
                    _dragOffset = SquadPostPiller.position - GetMouseWorldPos();
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) _isDragging = false;

        if (_isDragging)
        {
            Vector3 targetPos = GetMouseWorldPos() + _dragOffset;
            targetPos.y = SquadPostPiller.position.y;
            SquadPostPiller.position = targetPos;
        }
        _lineRenderer.enabled = _isDragging;
        
    }

    Vector3 GetMouseWorldPos()
    {
        Plane plane = new Plane(Vector3.up, new Vector3(0, SquadPostPiller.position.y, 0));
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float distance)) return ray.GetPoint(distance);
        return Vector3.zero;
    }

    // --- 2. Range & Trigger Logic ---
    void CheckMovementTrigger()
    {
        float distanceToSquad = Vector3.Distance(SquadPostPiller.position, _squadAnchorPosition);

        if (distanceToSquad > followTriggerRange)
        {
            MoveSquad();
            _squadAnchorPosition = SquadPostPiller.position;
        }
    }

    void UpdateRangeCircle()
    {
        float angleStep = 360f / _circleSegments;
        for (int i = 0; i < _circleSegments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle) * followTriggerRange, 0.2f, Mathf.Sin(angle) * followTriggerRange);
            _lineRenderer.SetPosition(i, SquadPostPiller.position + offset);
        }
    }

    // --- 3. Squad Movement ---
    void MoveSquad()
    {
        if (UnitList.Count == 0) return;

        Vector3 startOffset = CalculateCenterOffset();

        for (int i = 0; i < UnitList.Count; i++)
        {
            if (UnitList[i] == null) continue;

            Vector3 targetLocalPos = (currentFormation == FormationType.Line)
                                     ? new Vector3(i * unitSpacing, 0, 0)
                                     : GetBoxPosition(i);

            targetLocalPos -= startOffset;

            // TransformPoint ensures the formation follows the Pillar's rotation
            Vector3 targetWorldPos = SquadPostPiller.TransformPoint(targetLocalPos);
            UnitList[i].SetDestination(targetWorldPos);
        }
    }

    Vector3 GetBoxPosition(int index)
    {
        int row = index / boxColumns;
        int col = index % boxColumns;
        return new Vector3(col * unitSpacing, 0, -row * unitSpacing);
    }

    Vector3 CalculateCenterOffset()
    {
        float width, depth;
        if (currentFormation == FormationType.Line)
        {
            width = (UnitList.Count - 1) * unitSpacing;
            depth = 0;
        }
        else
        {
            int cols = Mathf.Min(UnitList.Count, boxColumns);
            int rows = Mathf.CeilToInt((float)UnitList.Count / boxColumns);
            width = (cols - 1) * unitSpacing;
            depth = (rows - 1) * unitSpacing;
        }
        return new Vector3(width / 2f, 0, -depth / 2f);
    }
}
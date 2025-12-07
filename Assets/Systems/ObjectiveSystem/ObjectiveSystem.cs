using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class ObjectiveSystem : MonoBehaviour
{
    [SerializeField] private List<Objective> _objectives;
    public static ObjectiveSystem Instance;
    private void Awake()
    {
        if (Instance != null && this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void AddObjective(Objective objective)
    {
        _objectives.Add(objective);
    }
    public bool UpdateObjective(int id, int amount)
    {
        var objective = GetObjective(id);
        if (objective != null)
        {
            objective.UpdateProgress(amount);
            return true;
        }
        return false;
    }

    public List<Objective> GetCompletedObjectives()
    {
        return _objectives.Where(o => o.Completed).ToList();
    }
    public List<Objective> GetIncompleteObjectives()
    {
        return _objectives.Where(o => !o.Completed).ToList();
    }
    public Objective GetObjective(int id)
    {
        return _objectives.Find((obj) => obj.ID == id);
    }
}

[System.Serializable]
public class Objective
{
    private static int _counter;
    private int _id;
    private int _progress;
    public int ID => _id;
    public string Description;
    public string Goal;
    public event System.Action OnComplete;
    public List<Tile> Tiles = new();
    public int Progress => _progress;
    public bool Completed
    {
        get
        {
            return Progress == 100;
        }
    }

    public int RemainingProgress
    {
        get
        { 
            return 100 - _progress; 
        }
    }
    public Objective(string description, string goal)
    {
        _id = Interlocked.Increment(ref _counter);
        Description = description;
        Goal = goal;
        _progress = 0;
    }
    public void UpdateProgress(int amount)
    {
        _progress += amount;
        _progress = Mathf.Clamp(_progress, 0, 100);
        if (_progress == 100)
        {
            OnComplete?.Invoke();
        }
    }
}

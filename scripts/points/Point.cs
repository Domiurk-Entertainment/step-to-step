using Godot;
using StepToStep.Systems;
using System;
using System.Collections.Generic;

namespace StepToStep.Level;

public partial class Point : Button
{
    public IReadOnlyCollection<Point> Points => _pointsUnlock;
    public int Visited
    {
        get => _visited;
        set{
            _visited = value;
            SaveSystem.Instance.SaveData(TypeConfiguration.Level,
                                         GetKey() + separator + Name + separator + nameof(Visited), _visited);
        }
    }

    [Export(PropertyHint.Dir)] public PackedScene SceneToLoad;
    [Export] public BattleConfig Config;

    [Export] private Point[] _pointsUnlock = Array.Empty<Point>();
    [Export] private int _canVisited = 1;
    private int _visited;
    private char separator = '/';

    private string GetKey() => GetTree().CurrentScene.Name;

    public override void _Ready()
    {
        Visited = SaveSystem.Instance.LoadIntData(TypeConfiguration.Level, GetKey() + nameof(Visited), _visited);
    }

    public bool CanVisit()
    {
        return _canVisited == -1 || _canVisited > Visited;
    }

    public IReadOnlyDictionary<string, string> KeysForSave => new Dictionary<string, string>(){
        { nameof(Visited), $"/{Name}/{nameof(Visited)}" },
    };
}
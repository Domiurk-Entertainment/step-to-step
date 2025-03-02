using Godot;
using System;
using System.Collections.Generic;

namespace StepToStep.Level;

public partial class Point : Button
{
    public IReadOnlyCollection<Point> Points => _pointsUnlock;
    public int Visited;

    [Export(PropertyHint.Dir)] public PackedScene SceneToLoad;
    [Export] public BattleConfig Config;

    [Export] private Point[] _pointsUnlock = Array.Empty<Point>();
    [Export] private int _canVisited = 1;

    public bool CanVisit()
    {
        return _canVisited == -1 || _canVisited > Visited;
    }

    public IReadOnlyDictionary<string, string> KeysForSave => new Dictionary<string, string>(){
        { nameof(Visited), $"/{Name}/{nameof(Visited)}" },
    };
}
using Godot;
using System;
using System.Collections.Generic;

namespace StepToStep.Level;

public partial class Point : Button
{
    public IReadOnlyCollection<Point> Points => _pointsUnlock;
    [Export] public BattleConfig Config;
    [Export(PropertyHint.Dir)] public PackedScene SceneToLoad;
    
    [Export] private Point[] _pointsUnlock = Array.Empty<Point>();

    public void ChangePointVisible(bool visible)
    {
        Disabled = !visible;
    }
}
using Godot;
using StepToStep.Systems;
using System;
using System.Collections.Generic;

namespace StepToStep.Levels;

public partial class Point : Button
{
	public IReadOnlyCollection<Point> Points => _pointsUnlock;
	public int Visited { get; set; }

	[Export(PropertyHint.Dir)] public PackedScene SceneToLoad;
	[Export] public BattleConfig Config;

	[Export] private Point[] _pointsUnlock = Array.Empty<Point>();
	[Export] private int _canVisited = 1;

	public bool CanVisit()
	{
		return _canVisited == -1 || _canVisited > Visited;
	}
}

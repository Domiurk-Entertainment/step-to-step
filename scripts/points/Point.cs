using Godot;
using StepToStep.Systems;
using System;
using System.Collections.Generic;

namespace StepToStep.Level;

public partial class Point : Button
{
	public IReadOnlyCollection<Point> Points => _pointsUnlock;
	public int Visited { get; set; }

	[Export(PropertyHint.Dir)] public PackedScene SceneToLoad;
	[Export] public BattleConfig Config;

	[Export] private Point[] _pointsUnlock = Array.Empty<Point>();
	[Export] public int _canVisited = 1;
	private char separator = '/';

	private string GetKey() => GetTree().CurrentScene.GetPath();

	public override void _EnterTree()
	{
		Visited = SaveSystem.Instance.LoadIntData(TypeConfiguration.Level, GetKey() + nameof(Visited), Visited);
	}

	public bool CanVisit()
	{
		return _canVisited == -1 || _canVisited > Visited;
	}
}

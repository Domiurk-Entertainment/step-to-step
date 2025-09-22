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
	[Export] public LevelConfig Config;

	[Export] private Point[] _pointsUnlock = Array.Empty<Point>();
	[Export] private int _canVisited = 1;

	public override void _Ready() {
		base._Ready();
		if(SceneToLoad != null && Config == null)
			GD.PrintErr($"{GetTree().CurrentScene.SceneFilePath}/{Name}:Config is null");
	}

	public bool CanVisit()
	{
		return _canVisited == -1 || _canVisited > Visited;
	}
}

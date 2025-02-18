using Godot;
using StepToStep.Battle;
using System;

namespace StepToStep.scripts;

public partial class Player : Node2D
{
	public event Action<StepType> ChangeStep;

	[Export] private Node2D spawnBalls;
	[Export] private Sight sight;
	[Export(PropertyHint.File)] public string ballScene;
	[Export] public float force = 500;

	public override void _Ready()
	{
		sight.CalculatedDirection += OnSightOnCalculatedDirection;
		return;

		void OnSightOnCalculatedDirection(Vector2 direction)
		{
			Ball instance = GD.Load<PackedScene>(ballScene).Instantiate() as Ball;
			spawnBalls.AddChild(instance);
			instance.Position = Vector2.Zero;
			instance.Throw(direction, force);
			ChangeStep?.Invoke(StepType.Attacked);
		}
	}

	public void Attack()
	{
		ChangeStep?.Invoke(sight.TryShoot() ? StepType.End : StepType.Start );
	}
}

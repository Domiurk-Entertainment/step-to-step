using Godot;
using StepToStep.Battle;
using System;

namespace StepToStep.scripts;

public partial class Player : StaticBody2D, IHealth
{
	public event Action<StepType> ChangeStep;

	[Export] private Node2D spawnBalls;
	[Export] private Sight sight;
	[Export(PropertyHint.File)] private string ballScene;
	[Export] private float force = 500;

	private Health health;

	public override void _Ready()
	{
		sight.CalculatedDirection += OnSightOnCalculatedDirection;
		health = GetNode<Health>("%Health");

		return;

		void OnSightOnCalculatedDirection(Vector2 direction)
		{
			Ball instance = GD.Load<PackedScene>(ballScene).Instantiate() as Ball;
			spawnBalls.AddChild(instance);
			instance.Position = Vector2.Zero;
			instance.Throw(direction, force);
			ChangeStep?.Invoke(StepType.Attacked);
			sight.Visible = false;
		}
	}

	public void Attack()
	{
		sight.Visible = true;
		ChangeStep?.Invoke(sight.TryShoot() ? StepType.End : StepType.Start);
	}

	public void TakeDamage(object sender, float damage)
	{
		if(damage <= 0)
			return;
		health.Subtract(sender, damage);
	}
}

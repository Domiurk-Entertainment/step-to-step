using Godot;
using System;
using System.ComponentModel;

namespace StepToStep.scripts;

public partial class Enemy : Node2D, IHealth
{
	public event Action<StepType> ChangeStep;
	[Export, Category("Tween")] private float duration = 1;
	[Export] private Tween.TransitionType transitionType = Tween.TransitionType.Linear;
	[Export] private float attackRange = 15;
	[Export] private RayCast2D rayCast2D;

	private Tween movingTween;
	private Vector2 position;
	private Health health;

	public override void _Ready()
	{
		health = GetNode<Health>("%Health");
		ChangeStep += OnChangeStep;
	}

	public bool TryAttack(Vector2 playerPosition)
	{
		ChangeStep?.Invoke(StepType.Start);
		position = GlobalPosition;

		CreateToPlayer();

		return true;

		void CreateToPlayer()
		{
			movingTween?.Kill();
			movingTween = CreateTween();

			movingTween.TweenProperty(this, "global_position", playerPosition,
									  duration)
					   .From(GlobalPosition)
					   .SetTrans(transitionType);
			movingTween.Finished += Attacked;
		}

		void CreateFromPlayer()
		{
			movingTween?.Kill();
			movingTween = CreateTween();

			movingTween.TweenProperty(this, "global_position", position, duration)
					   .From(GlobalPosition)
					   .SetTrans(transitionType);
			movingTween.Finished += () => ChangeStep?.Invoke(StepType.End);
		}

		void Attacked()
		{
			ChangeStep?.Invoke(StepType.Attacked);
			CreateFromPlayer();
		}
	}

	private void MoveToPlayer(Vector2 playerPosition)
	{
		movingTween.TweenProperty(this, "global_position", playerPosition, duration)
				   .From(GlobalPosition)
				   .SetTrans(transitionType);
	}

	private void OnChangeStep(StepType stepType)
	{
		switch(stepType){
			case StepType.Start:
				break;
			case StepType.Attacked:
				if(rayCast2D.IsColliding()){
					var target = rayCast2D.GetCollider().GetScript();
					GD.Print("Target: " + target);
				}

				break;
			case StepType.End:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(stepType), stepType, null);
		}
	}

	public void TakeDamage(object sender, float damage)
	{
		if(damage <= 0)
			return;
		health.Subtract(sender, damage);
	}
}

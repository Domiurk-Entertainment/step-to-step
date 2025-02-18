using Godot;
using System;
using System.ComponentModel;

namespace StepToStep.scripts;

public partial class Enemy : Node2D
{
	public event Action<StepType> ChangeStep;
	[Export, Category("Tween")] private float duration = 1;
	[Export] private Tween.TransitionType transitionType = Tween.TransitionType.Linear;

	private Tween movingTween;
	private Vector2 position;

	public override void _Ready() { }

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
		CreateTween();

		movingTween.TweenProperty(this, "global_position", playerPosition, duration)
				   .From(GlobalPosition)
				   .SetTrans(transitionType);
	}
}

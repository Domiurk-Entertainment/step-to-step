using Godot;
using System;
using System.ComponentModel;

namespace StepToStep.Battle;

public partial class Sight : Node2D
{
	private const int UP = 1;
	private const int DOWN = -1;
	private const int STOP = 0;
	private const int PLAY = 1;

	private const string PROPERTY_NAME = "rotation_degrees";
	public event Action<Vector2> CalculatedDirection;

	[Export, ExportCategory("Settings")] private float minValue = -60;
	[Export] private float maxValue = 60;
	[Category("Tween")]
	[Export] private float duration = 1;
	[Export] private Tween.TransitionType transition = Tween.TransitionType.Linear;

	[Export, ExportCategory("Settings/Nodes")]
	public Node2D StartPoint;
	[Export] public Node2D EndPoint;

	private int state;
	private int direction;

	private Tween tween;

	public override void _Ready()
	{
		RotationDegrees = minValue;
		state = STOP;
		direction = UP;
	}

	private Vector2 Calculate() => (EndPoint.GlobalPosition - StartPoint.GlobalPosition).Normalized();

	public bool TryShoot()
	{
		if(state == PLAY){
			EndRotation();
			return true;
		}

		tween = ContinueTween();
		state = PLAY;
		return false;
	}

	private void EndRotation()
	{
		state = STOP;
		tween?.Kill();
		CalculatedDirection?.Invoke(Calculate());
	}

	private Tween CreateNewTween()
	{
		Tween result = CreateTween();

		if(RotationDegrees <= minValue){
			SetProperty(result, minValue, maxValue);
			direction = UP;
		}
		else{
			SetProperty(result, maxValue, minValue);
			direction = DOWN;
		}

		result.Finished += () => tween = CreateNewTween();
		return result;
	}

	private Tween ContinueTween()
	{
		Tween result = CreateTween();
		SetProperty(result, RotationDegrees, direction == DOWN ? minValue : maxValue);
		result.Finished += () => tween = CreateNewTween();
		return result;
	}

	private void SetProperty(Tween tween, float from, float to) => tween
																   .TweenProperty(this, PROPERTY_NAME, to, duration)
																   .From(from)
																   .SetTrans(transition);
}

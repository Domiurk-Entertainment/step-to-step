using Godot;
using StepToStep.InventorySystem;
using StepToStep.Utils;
using System;
using System.ComponentModel;

namespace StepToStep.Entity;

public partial class Enemy : EnemyBase, IHealth
{
	public event Action<AttackType> AttackedStep;


	[Export] public Item Reward;
	private Health _health;
	private AnimatedSprite2D _animatedSprite;

	#region Attack

	[Export]
	private float _damage = 10;
	[Export] private float _attackRange = 15;
	[Export, Category("Tween")] private float _duration = 1;
	[Export] private Tween.TransitionType _transitionType = Tween.TransitionType.Linear;
	[Export] private RayCast2D _rayCast2D;
	[Export] private int _stepCount = 2;

	private string _attackAnimationName = "attack";
	private Tween _movingTween;
	private Vector2[] _steps;
	private int _currentStep;

	public void InitialTarget(Vector2 targetPosition)
	{
		if(_steps != null && _steps.Length < _stepCount)
			return;

		_steps = new Vector2[_stepCount + 1];
		Vector2 way = (targetPosition - GlobalPosition) / _stepCount;
		way = new Vector2(way.X + _attackRange, way.Y);
		_rayCast2D.TargetPosition = new Vector2(way.Normalized().X * _attackRange, way.Normalized().Y);

		for(int i = 0; i <= _stepCount; i++){
			if(i == 0){
				_steps[i] = GlobalPosition;
				continue;
			}

			_steps[i] = GlobalPosition + way * i;
		}

		_steps[_stepCount] = new Vector2(_steps[_stepCount].X, _steps[_stepCount].Y);
	}

	private void MoveTo(Vector2 toPosition)
	{
		_movingTween?.Kill();
		_movingTween = CreateTween();

		_movingTween.TweenProperty(this, "global_position", toPosition,
								   _duration)
					.From(GlobalPosition)
					.SetTrans(_transitionType);
		_movingTween.Finished += CheckReturnOnFinished;
		return;

		void CheckReturnOnFinished()
		{
			if(_currentStep != _stepCount){
				AttackedStep?.Invoke(AttackType.End);
				return;
			}

			_animatedSprite.Play(_attackAnimationName);
		}
	}

	public void Attack()
	{
		_currentStep++;
		AttackedStep?.Invoke(AttackType.Start);

		MoveTo(_steps[_currentStep]);
	}

	private void Attacked()
	{
		if(_rayCast2D.GetCollider() is not IHealth health)
			return;

		AttackedStep?.Invoke(AttackType.Attacked);
		health.TakeDamage(this, _damage);
		MoveTo(_steps[_currentStep = 0]);
	}

	#endregion

	public override void _Ready()
	{
		_health = GetNode<Health>("%Health");
		_animatedSprite = (AnimatedSprite2D)FindChild("Visual");

		_health.ChangedValue += HealthOnChangedValue;
		_health.DecreasedValue += DecreasedValue;
		_animatedSprite.FrameChanged += AnimatedSpriteOnSpriteFramesChanged;
		_animatedSprite.AnimationFinished += AnimatedSpriteOnAnimationFinished;
		return;

		void DecreasedValue()
			=> EmitSignal(SignalName.Hit);
	}

	private void AnimatedSpriteOnAnimationFinished()
	{
		if(_animatedSprite.GetAnimation() != _attackAnimationName)
			return;
		AttackedStep?.Invoke(AttackType.End);
		_animatedSprite.Play("idle");
	}

	private void AnimatedSpriteOnSpriteFramesChanged()
	{
		if(_animatedSprite.GetAnimation() != _attackAnimationName || _animatedSprite.Frame != 2)
			return;
		Attacked();
	}

	/*
	public void TakeDamage(Node sender, float damage)
	{
		if(damage <= 0)
			return;
		_health.Subtract(sender, damage);
		_animatedSprite.Play("hit");
	}
	*/

	private void HealthOnChangedValue(float newValue)
	{
		if(newValue > _health.MinValue)
			return;
		EmitSignal(SignalName.Dead);
	}
}

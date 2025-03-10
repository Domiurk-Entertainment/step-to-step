using Godot;
using StepToStep.Utils;
using System;

namespace StepToStep.Entity;

public partial class BossCave : Node2D, IHealth
{
	public event Action<AttackType> AttackedStep;

	[Signal] public delegate void DeadEventHandler();

	[Signal] public delegate void HitEventHandler();

	[Export] private float Damage;
	[Export] private AnimatedSprite2D _animatedSprite;
	[Export] private string attackAnimationName = "attack";
	[Export] private Node2D _pointToShoot;
	[Export] private PackedScene _projectile;
	[Export] private Health _health;
	private Vector2 _playerPosition;

	public override void _Ready()
	{
		_animatedSprite.FrameChanged += AnimatedSpriteOnFrameChanged;
		_animatedSprite.AnimationFinished += AnimatedSpriteOnAnimationFinished;
		_health.ChangedValue += HealthOnChangedValue;
		_health.DecreasedValue += DecreasedValue;
		return;

		void DecreasedValue()
			=> EmitSignal(SignalName.Hit);
	}

	private void AnimatedSpriteOnAnimationFinished()
	{
		if(_animatedSprite.GetAnimation() != "attack")
			return;
		AttackedStep?.Invoke(AttackType.End);
		_animatedSprite.Play("idle");
	}

	public void InitialTarget(Vector2 targetPosition)
	{
		_playerPosition = targetPosition;
	}

	private void AnimatedSpriteOnFrameChanged()
	{
		if(_animatedSprite.GetAnimation() != "attack" || _animatedSprite.Frame != 7)
			return;

		var projectile = _projectile.Instantiate<Projectile>();
		_pointToShoot.AddChild(projectile);
		projectile.Position = Vector2.Zero;
		projectile.Shoot(_playerPosition, Damage);
		AttackedStep?.Invoke(AttackType.Attacked);
	}

	public void Attack()
	{
		AttackedStep?.Invoke(AttackType.Start);
		_animatedSprite.Play("attack");
	}

	public void TakeDamage(Node sender, float damage)
	{
		if(damage <= 0)
			return;
		_health.Subtract(sender, damage);
		_animatedSprite.Play("hit");
	}

	private void HealthOnChangedValue(float newValue)
	{
		if(newValue > _health.MinValue)
			return;
		EmitSignal(SignalName.Dead);
	}
}

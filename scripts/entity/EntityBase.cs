using Godot;
using StepToStep.Utils;

namespace StepToStep.Entity;

public partial class EntityBase : Node2D, IHealth
{
	[Signal] public delegate void DeadEventHandler();
	[Signal] public delegate void HitEventHandler();

	[Export] public AnimationPlayer Animator { get; private set; }
	[ExportGroup("AnimationsName")]
	[Export] private string _hitAnimationName = "hit";
	private Health _health;

	public override void _Ready()
	{
		_health = this.FindNode<Health>();
		Animator = this.FindNode<AnimationPlayer>();
		_health.ChangedValue += HealthOnChangedValue;
		_health.DecreasedValue += DecreasedValue;
		return;

		void DecreasedValue()
			=> EmitSignal(SignalName.Hit);
	}

	private void HealthOnChangedValue(float newValue)
	{
		if(newValue > _health.MinValue)
			return;
		EmitSignal(SignalName.Dead);
	}

	public void TakeDamage(Node sender, float damage)
	{
		if(damage <= 0)
			return;
		_health.Subtract(sender, damage);
		if(Animator.HasAnimation(_hitAnimationName))
			Animator.Play(_hitAnimationName);
	}
}

using Godot;
using StepToStep.Utils;

namespace StepToStep.Entity;

public partial class EntityBase : Node2D, IHealth {
    [Signal] public delegate void DeadEventHandler();
    [Signal] public delegate void HitEventHandler();

    [ExportGroup("AnimationsName")]
    [Export] public AnimatedSprite2D AnimatedSprite;
    [ExportGroup("AnimationsName")]
    [Export] private string _hitAnimationName = "hit";
    [Export] private Health _health;

    public override void _Ready() {
        _health = this.FindNode<Health>();
        AnimatedSprite = this.FindNode<AnimatedSprite2D>();
        _health.ChangedValue += HealthOnChangedValue;
        _health.DecreasedValue += DecreasedValue;
        return;

        void DecreasedValue() => EmitSignal(SignalName.Hit);
    }

    private void HealthOnChangedValue(float newValue) {
        if (newValue > _health.MinValue)
            return;
        EmitSignal(SignalName.Dead);
    }

    public void TakeDamage(Node sender, float damage) {
        if (damage <= 0)
            return;
        GD.Print(_health);
        _health.Subtract(sender, damage);
        PlayAnimation(_hitAnimationName);
    }

    public void PlayAnimation(string animationName) {
        if (AnimatedSprite.SpriteFrames.HasAnimation(animationName))
            AnimatedSprite.Play(animationName);
        else
            GD.PrintErr($"{Name}: Not Contains {animationName} Animation");
    }
}

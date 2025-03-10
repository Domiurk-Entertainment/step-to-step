using Godot;
using StepToStep.Utils;

namespace StepToStep.Entity;

public partial class Projectile : Area2D
{
    [Signal] public delegate void HitEventHandler();

    [Export] private Tween.TransitionType _tweenType = Tween.TransitionType.Linear;
    [Export] private float duration = 1;
    [Export] private Vector2 offset = new(0, -20);
    private Tween _moveTween;
    private float _damage;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if(body is not IHealth health){
            return;
        }

        health.TakeDamage(this, _damage);
        EmitSignal(SignalName.Hit);
        QueueFree();
    }

    public void Shoot(Vector2 endPosition, float damage)
    {
        _moveTween = CreateTween();
        _moveTween.TweenProperty(this, "global_position", endPosition + offset, duration)
                  .SetTrans(_tweenType);
        _damage = damage;
    }
}
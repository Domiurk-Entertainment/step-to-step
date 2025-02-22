using Godot;
using StepToStep.Inventory;
using StepToStep.scripts;
using System;

namespace StepToStep;

public partial class Ball : CharacterBody2D
{
    protected event Action<Node2D> Hit;

    [Export] public BallResource Resource;
    [Export] private float damage = 1;

    public void Throw(Vector2 direction, float force)
    {
        direction = direction.Normalized();
        Velocity = direction * force;
    }

    public override void _PhysicsProcess(double delta)
    {
        if(Velocity is{ X: 0, Y: 0 })
            return;
        KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);

        if(collision == null)
            return;
        Vector2 normal = collision.GetNormal();
        Velocity = Velocity.Bounce(normal.Normalized());
    }

    private void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }

    private void OnHit(Node2D body)
    {
        if(body is not IHealth health)
            return;

        health.TakeDamage(this, damage);
        Hit?.Invoke(body);
    }
}
using Godot;
using StepToStep.InventorySpace;
using StepToStep.scripts;
using StepToStep.Utils;
using System;

namespace StepToStep;

public partial class Ball : CharacterBody2D
{
    protected event Action<Node2D> Hit;

    private float _damage;

    public void Throw(Vector2 direction, float force, float damage)
    {
        direction = direction.Normalized();
        Velocity = direction * force;
        _damage = damage;
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
        if(body is not IHealth health){
            return;
        }
        
        health.TakeDamage(this, _damage);
        Hit?.Invoke(body);
    }
}
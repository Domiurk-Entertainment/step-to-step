using Godot;

namespace StepToStep;

public partial class Ball : RigidBody2D
{
    private const float FORCE = 50;

    public void Throw(Vector2 direction, float force = FORCE)
    {
        LinearVelocity = direction.Normalized() * FORCE;
        ApplyForce();
    }
}
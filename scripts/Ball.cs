using Godot;

namespace StepToStep;

public partial class Ball : CharacterBody2D
{
    private const float FORCE = 500;
    private float force = 500;

    public void Throw(Vector2 direction, float force = FORCE)
    {
        direction = direction.Normalized() +
                    ProjectSettings.GetSetting("physics/2d/default_gravity_vector").AsVector2();
        Velocity = direction * force;
        this.force = force;
    }

    public override void _PhysicsProcess(double delta)
    {
        if(Velocity is{ X: 0, Y: 0 })
            return;
        KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);

        if(collision != null){
            Vector2 normal = collision.GetNormal();
            Velocity = Velocity.Bounce(normal.Normalized());
        }
    }

    private void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }
}
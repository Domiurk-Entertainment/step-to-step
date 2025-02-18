using Godot;

namespace StepToStep;

public partial class Ball : CharacterBody2D
{
	public void Throw(Vector2 direction, float force)
	{
		direction = direction.Normalized();
					// + ProjectSettings.GetSetting("physics/2d/default_gravity_vector").AsVector2();
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
}

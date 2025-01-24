using Godot;

namespace StepToStep;

public partial class Point : Sprite2D
{
	public override void _Ready()
	{
		base._Ready();
		
		GD.Print(Transform.Origin.X);
		GD.Print(Transform.Origin.Y);
	}
}

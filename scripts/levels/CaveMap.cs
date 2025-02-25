using Godot;
using StepToStep.Level;
using StepToStep.Utils;

public partial class CaveMap : Node2D
{
	
	[Export] private Node2D _player;

	public override void _Ready()
	{
		// PointsService test = this.FindNode<PointsService>();
		// GD.Print(test);
	}
}

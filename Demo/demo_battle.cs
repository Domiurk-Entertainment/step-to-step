using Godot;
using StepToStep;
using System;

public partial class demo_battle : Node2D
{
	[Export] private Node2D player;
	private Vector2 start_Position;
	private Ball ball;
	[Export] private HSlider x;
	[Export] private HSlider y;
	[Export] private TextEdit force;
	[Export] private Label labelx;
	[Export] private Label labely;
	
	public override void _Ready()
	{
		ball = GetNode<Ball>("Ball");
		start_Position = ball.GlobalPosition;
		x.ValueChanged += v => labelx.Text = v.ToString("#.#");
		y.ValueChanged += v => labely.Text = v.ToString("#.#");
	}

	public override void _Input(InputEvent @event)
	{
		if(Input.IsKeyPressed(Key.I))
			GetTree().CurrentScene.PrintTree();

		if(ball is null)
			return;
		
		if(Input.IsKeyPressed(Key.Y)){
			ball.Throw(new Vector2((float)x.Value, (float)y.Value),int.Parse(force.Text));
		}
		if(Input.IsKeyPressed(Key.R))
			ball.GlobalPosition = start_Position;
	}
}

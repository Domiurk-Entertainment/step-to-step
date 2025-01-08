using Godot;

namespace StepToStep.Demo;

public partial class Demo_Battle : Node2D
{
    [Export] private Node2D player;
    private Vector2 start_Position;
    private Ball ball;
    [Export] private TextEdit forceText;

    public override void _Ready()
    {
        ball = player.GetNode<Ball>("Ball");
        start_Position = ball.GlobalPosition;
    }

    public override void _Input(InputEvent @event)
    {
        if(Input.IsKeyPressed(Key.Y)){
            ball.Throw(Vector2.Right,10);
        }

        if(Input.IsKeyPressed(Key.R))
            ball.GlobalPosition = start_Position;
    }
}
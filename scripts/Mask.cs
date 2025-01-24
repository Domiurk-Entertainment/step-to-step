using Godot;

namespace StepToStep;

public partial class Mask : Sprite2D
{
    [Export] private Node2D player;
    [Export] private Vector2 offset;

    public override void _Ready()
    {
        base._Ready();
        if(player is null)
            GD.Print($"Player is not found. {GetTree().CurrentScene.Name} -> {GetTree()}");
    }

    public override void _Process(double delta)
    {
        if(player is null)
            return;
        GlobalPosition = player.GlobalPosition + offset;
    }
}
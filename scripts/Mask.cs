using Godot;
using Godot.Collections;

namespace StepToStep;

public partial class Mask : Sprite2D
{
    [Export] private Node2D player;
    [Export] private Vector2 offset;

    public override void _Process(double delta)
    {
        GlobalPosition = player.GlobalPosition + offset;
    }
}
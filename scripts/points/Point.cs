using Godot;
using System;

namespace StepToStep;

public partial class Point : Node2D
{
    public event Action<Vector2> ClickedToPoint;
    [Export]
    private Button button;
    [Export]
    private Vector2 offset = new Vector2(30, -25);

    private void OnPressed()
    {
        ClickedToPoint?.Invoke(GlobalPosition + offset);
        button.ReleaseFocus();
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        button.Pressed += OnPressed;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        button.Pressed -= OnPressed;
    }
}
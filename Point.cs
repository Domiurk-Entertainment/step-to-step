using Godot;

namespace StepToStep;

public partial class Point : TouchScreenButton
{
    public override void _Ready()
    {
        base._Ready();
        this.Pressed += PressedToPoint;
    }

    private void PressedToPoint()
    {
        GD.Print(Transform.X);    
        GD.Print(Transform.Y);    
    }
}
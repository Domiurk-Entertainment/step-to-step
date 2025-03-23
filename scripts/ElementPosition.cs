using Godot;

namespace StepToStep;

public partial class ElementPosition : Node2D
{
    [Export] private float _percentage = -0.025f;
    [Export] private float _dividerX = 2;
    [Export] private float _dividerY = 2;

    public override void _Ready()
    {
        float x = (float)ProjectSettings.GetSetting("display/window/size/viewport_width").AsDouble();
        float y = (float)ProjectSettings.GetSetting("display/window/size/viewport_height").AsDouble();
        GlobalPosition = new Vector2(x / _dividerX, y / _dividerY + x * _percentage);
    }
}
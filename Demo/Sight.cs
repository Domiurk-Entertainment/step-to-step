using Godot;
using System;

namespace StepToStep.Battle;

public partial class Sight : Node2D
{
    public event Action<Vector2> CalculatedDirection;

    [Export] private float minValue = -90;
    [Export] private float maxValue = 0;
    [Export] private float duration = 1;
    [Export] private Tween.TransitionType transition = Tween.TransitionType.Linear;
    [Export] public Node2D StartPoint;
    [Export] public Node2D EndPoint;
    private int state;
    private Tween tween;
    private Vector2 Calculate() => (EndPoint.GlobalPosition - StartPoint.GlobalPosition).Normalized();

    public void Starting()
    {
        if(state == 1){
            EndRotation();
            return;
        }

        tween = CreateNewTween();
        state = 1;
    }

    private void EndRotation()
    {
        state = 0;
        tween?.Kill();
        CalculatedDirection?.Invoke(Calculate());
    }

    private Tween CreateNewTween()
    {
        const string property_name = "rotation_degrees";
        Tween result = CreateTween();
        if(RotationDegrees <= minValue)
            result.TweenProperty(this, property_name, maxValue, duration)
                  .From(minValue)
                  .SetTrans(transition);
        else
            result.TweenProperty(this, property_name, minValue, duration)
                  .From(maxValue)
                  .SetTrans(transition);
        result.Finished += () => tween = CreateNewTween();
        return result;
    }
}
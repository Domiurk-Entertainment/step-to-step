using Godot;

namespace StepToStep.Scripts.StatHandler;

[GlobalClass]
public partial class Attribute : Stat
{
    [Export] private float maxValue;
    public float MaxValue
    {
        get => maxValue;
        internal set => maxValue += value;
    }

    internal override void AddValue(float value)
    {
        Value = Mathf.Min(Value + value, maxValue);
    }
}
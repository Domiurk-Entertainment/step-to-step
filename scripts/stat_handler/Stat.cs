using Godot;

namespace StepToStep.Scripts.StatHandler;

[GlobalClass]
public partial class Stat : Node
{
    [Export] public float Value { get; protected set; }

    internal virtual void AddValue(float value)
    {
        Value += value;
    }
}
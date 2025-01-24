using Godot;

namespace StepToStep.Stats;

[GlobalClass]
public partial class Stat : Node
{
    [Signal] public delegate void UpdateStatValueEventHandler(string name, int value);

    [Export] private int value;

    public void AddPoint()
    {
        value++;
        EmitSignal(SignalName.UpdateStatValue, Name, value);
    }

    public int Value => value;
}
using Godot;
using System;

namespace StepToStep;

[GlobalClass]
public partial class Health : ProgressBar
{
    public event Action<float> ChangedValue;

    public void Subtract(object sender, double value)
    {
        if(value < 0)
            throw new Exception($"{sender} sending failure value ({value})");
        Value -= (value);
        ChangedValue?.Invoke((float)Value);
    }

    public void Add(object sender, double value)
    {
        if(value < 0)
            throw new Exception($"{sender} sending failure value ({value})");
        Value += value;
        ChangedValue?.Invoke((float)Value);
    }
}
using Godot;
using System;
using System.Reflection;

namespace StepToStep;

[GlobalClass]
public partial class Health : ProgressBar
{
    public void Subtract(object self, double value)
    {
        if(value < 0)
            throw new Exception($"{self} sending failure value ({value})");
        Value -= (value);
    }

    public void Add(object self, double value)
    {
        if(value < 0)
            throw new Exception($"{self} sending failure value ({value})");
        Value += value;
    }
}
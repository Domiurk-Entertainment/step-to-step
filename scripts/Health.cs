using Godot;
using System;
using System.Reflection;

namespace StepToStep;

[GlobalClass]
public partial class Health : ProgressBar
{
    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if(Input.IsKeyPressed(Key.A)){
            Add(this, 10);
        }

        if(Input.IsKeyPressed(Key.D))
            Subtract(this, 10);

        if(Input.IsKeyPressed(Key.S)){
            GD.Print(Value);
        }
    }

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
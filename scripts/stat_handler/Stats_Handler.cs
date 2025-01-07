using Godot;
using System;
using System.Collections.Generic;

namespace StepToStep.Scripts.StatHandler;

public partial class Stats_Handler : Node
{
    private List<Stat> stats = new();

    public override void _Ready()
    {
        foreach(Node variable in GetChildren()){
            try{
                stats.Add(variable as Stat);
            }
            catch(Exception e){
                GD.Print($"{variable.Name} is not a {typeof(Stat)} Node. Exception {e}");
            }
        }
    }

    public void Apply(Stat_Data data)
    {
    }
}
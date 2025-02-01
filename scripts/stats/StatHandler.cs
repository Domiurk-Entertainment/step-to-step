using Godot;
using System.Collections.Generic;
using System.Linq;

namespace StepToStep.Stats;

public partial class StatHandler : Node
{
    private readonly Dictionary<string, Stat> stats = new Dictionary<string, Stat>();

    public override void _Ready()
    {
        base._Ready();

        foreach(Stat child in GetChildren().Where(c => c is Stat)){
            stats.Add(child.Name, child);
        }
    }

    public bool TryGet(string name, out Stat stat)
    {
        stat = default;
        bool result = stats.TryGetValue(name, out stat);
        return result;
    }

    public void Add(string name)
    {
        if(TryGet(name, out Stat stat))
            stat.AddPoint();
    }

    public Stat[] All() => stats.Values.ToArray();
}
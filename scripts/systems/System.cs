using Godot;

namespace StepToStep.Systems;

public partial class System<T> : Node where T : Node
{
    public static T Instance { get; private set; }

    public override void _Ready()
    {
        if(Instance != null && Instance != this)
            QueueFree();
        else
            Instance = this as T;
    }
}
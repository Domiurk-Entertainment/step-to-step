using Godot;

namespace StepToStep.Inventory;

[GlobalClass]
public partial class BallResource : ItemResource
{
    [Export] private float damage = 1;

    [Export(PropertyHint.Dir)] public string PackedScene { get; private set; }
    public float Damage => damage;
}
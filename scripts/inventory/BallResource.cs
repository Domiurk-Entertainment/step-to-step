using Godot;

namespace StepToStep.InventorySystem;

[GlobalClass]
public partial class BallResource : ItemResource
{
    [Export] public PackedScene PackedScene { get; private set; }
    [Export] public int Cost { get; private set; }
    [Export] public float Damage { get; private set; } = 1;
}
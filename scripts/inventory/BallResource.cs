using Godot;

namespace StepToStep.InventorySpace;

[GlobalClass]
public partial class BallResource : ItemResource
{
    [field: Export] public PackedScene PackedScene { get; private set; }
    [field: Export] public int Cost { get; private set; }
    [field: Export] public float Damage { get; private set; } = 1;
}
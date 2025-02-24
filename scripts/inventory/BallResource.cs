using Godot;

namespace StepToStep.InventorySpace;

[GlobalClass]
public partial class BallResource : ItemResource
{
    [Export] private float _damage = 1;
    [Export]public PackedScene PackedScene { get; private set; }
    public float Damage => _damage;
}
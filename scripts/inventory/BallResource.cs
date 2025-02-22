using Godot;

namespace StepToStep.Inventory;

[GlobalClass]
public partial class BallResource : ItemResource
{
    [Export] private float _damage = 1;

    [Export(PropertyHint.File)] public string PackedScene { get; private set; }
    public float Damage => _damage;
}
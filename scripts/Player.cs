using Godot;
using StepToStep.Battle;
using System.Threading.Tasks;

namespace StepToStep.scripts;

public partial class Player : Entity
{
    [Export] private Node2D spawnBalls;
    [Export] public Sight sight;
    [Export(PropertyHint.File)] public string ballScene;
    [Export] public float force = 500;

    public override void _Ready()
    {
        sight.CalculatedDirection += OnSightOnCalculatedDirection;
        return;
        void OnSightOnCalculatedDirection(Vector2 direction)
        {
            Ball instance = GD.Load<PackedScene>(ballScene).Instantiate() as Ball;
            spawnBalls.AddChild(instance);
            instance.Position = Vector2.Zero;
            instance.Throw(direction, force);
        }
    }

    public override void Attack() { }
}
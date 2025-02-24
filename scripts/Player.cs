using Godot;
using StepToStep.Battle;
using StepToStep.InventorySpace;
using StepToStep.scripts;
using StepToStep.Utilities;
using StepToStep.Utils;
using System;

namespace StepToStep;

public partial class Player : StaticBody2D, IHealth
{
    public event Action<StepType> ChangeStep;

    public IInventory Inventory => _inventory;

    [Export] private Node2D _spawnBalls;
    [Export] private Sight _sight;
    [Export] private float _force = 500;

    private Inventory _inventory;
    private Health _health;

    public override void _Ready()
    {
        _sight.CalculatedDirection += OnSightOnCalculatedDirection;
        _health = GetNode<Health>("%Health");
        _inventory = GetNode<Inventory>("%Inventory");

        return;

        void OnSightOnCalculatedDirection(Vector2 direction)
        {
            Item ball = Inventory.GetBall();

            if(ball == null){
                GD.Print("Fuck");
                return;
            }

            Ball instance = ball.Resource.PackedScene.Instantiate<Ball>();
            _spawnBalls.AddChild(instance);
            instance.Position = Vector2.Zero;
            instance.Throw(direction, _force);
            ChangeStep?.Invoke(StepType.Attacked);
            _sight.Visible = false;
        }
    }

    public void Attack()
    {
        bool tryShoot = _sight.TryShoot();
        ChangeStep?.Invoke(tryShoot ? StepType.End : StepType.Start);
        _sight.Visible = !tryShoot;
    }

    public void TakeDamage(object sender, float damage)
    {
        if(damage <= 0)
            return;
        _health.Subtract(sender, damage);
    }
}
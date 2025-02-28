using Godot;
using StepToStep.Battle;
using StepToStep.InventorySpace;
using StepToStep.Utilities;
using StepToStep.Utils;
using System;

namespace StepToStep;
[GlobalClass,Icon("res://sprites/player_mini.png")]
public partial class Player : StaticBody2D, IHealth
{
    public event Action<AttackType> AttackedStep;

    [Signal] public delegate void DeadEventHandler();

    public IInventory Inventory => _inventory;

    [Export] public float ChanceToRun;
    
    [Export] private Node2D _spawnBalls;
    [Export] private Sight _sight;
    [Export] private float _force = 500;

    private Inventory _inventory;
    private Health _health;

    public override void _Ready()
    {
        _health = GetNode<Health>("%Health");
        _inventory = GetNode<Inventory>("%Inventory");

        _sight.CalculatedDirection += OnSightOnCalculatedDirection;
        _health.ChangedValue += HealthOnChangedValue;
    }

    private void OnSightOnCalculatedDirection(Vector2 direction)
    {
        Item ball = Inventory.GetBall();

        if(ball == null){
            GD.Print("Fuck");
            return;
        }

        Ball instance = ball.Resource.PackedScene.Instantiate<Ball>();
        _spawnBalls.AddChild(instance);
        instance.Position = Vector2.Zero;
        instance.Throw(direction, _force, ball.Resource.Damage);
        AttackedStep?.Invoke(AttackType.Attacked);
        _sight.Visible = false;
    }

    public override void _ExitTree()
    {
        _sight.CalculatedDirection -= OnSightOnCalculatedDirection;
    }

    public void Attack()
    {
        bool tryShoot = _sight.TryShoot();
        AttackedStep?.Invoke(tryShoot ? AttackType.End : AttackType.Start);
        _sight.Visible = !tryShoot;
    }

    public void TakeDamage(object sender, float damage)
    {
        if(damage <= 0)
            return;
        _health.Subtract(sender, damage);
    }

    private void HealthOnChangedValue(float newValue)
    {
        if(newValue > _health.MinValue)
            return;
        EmitSignal(SignalName.Dead);
    }
}
 using Godot;
using StepToStep.Battle;
using StepToStep.InventorySystem;
using StepToStep.Utils;
using System;

namespace StepToStep.Entity;

[GlobalClass, Icon("res://sprites/player_mini.png")]
public partial class Player : EntityBase, IAttack
{
    public event Action<AttackType> AttackedStep;

    public IInventory Inventory => _inventory;

    [Export] public float ChanceToRun;

    [Export] private Node2D _spawnBalls;
    [Export] private Sight _sight;
    [Export] private float _force = 500;

    private Inventory _inventory;

    public override void _Ready()
    {
        base._Ready();
        _inventory = this.FindNode<Inventory>();

        _sight.CalculatedDirection += OnSightOnCalculatedDirection;
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
}
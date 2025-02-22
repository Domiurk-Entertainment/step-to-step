using Godot;
using StepToStep.Battle;
using System;

namespace StepToStep.scripts;

public partial class Player : StaticBody2D, IHealth
{
    public event Action<StepType> ChangeStep;

    public string BallScene = "res://scenes/balls/base_ball.tscn";
    public bool CanShoot;

    [Export] private Node2D _spawnBalls;
    [Export] private Sight _sight;
    [Export] private float _force = 500;

    private Health _health;

    public override void _Ready()
    {
        _sight.CalculatedDirection += OnSightOnCalculatedDirection;
        _health = GetNode<Health>("%Health");

        return;

        void OnSightOnCalculatedDirection(Vector2 direction)
        {
            Ball instance = GD.Load<PackedScene>(BallScene).Instantiate() as Ball;
            _spawnBalls.AddChild(instance);
            instance.Position = Vector2.Zero;
            instance.Throw(direction, _force);
            ChangeStep?.Invoke(StepType.Attacked);
            _sight.Visible = false;
        }
    }

    public void Attack()
    {
        _sight.Visible = true;
        ChangeStep?.Invoke(_sight.TryShoot() ? StepType.End : StepType.Start);
    }

    public void TakeDamage(object sender, float damage)
    {
        if(damage <= 0)
            return;
        _health.Subtract(sender, damage);
    }
}
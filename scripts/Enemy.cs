using Godot;
using StepToStep.Utils;
using System;
using System.ComponentModel;

namespace StepToStep.scripts;

public partial class Enemy : Node2D, IHealth
{
    public event Action<AttackType> AttackedStep;
    [Signal] public delegate void DeadEventHandler();


    [Export, Category("Tween")] private float _duration = 1;
    [Export] private Tween.TransitionType _transitionType = Tween.TransitionType.Linear;
    [Export] private float _attackRange = 15;
    [Export] private float _damage = 10;
    [Export] private RayCast2D _rayCast2D;

    private Tween _movingTween;
    private Vector2 _position;
    private Health _health;

    public override void _Ready()
    {
        _health = GetNode<Health>("%Health");
        
        _health.ChangedValue += HealthOnChangedValue;
        AttackedStep += OnAttackedStep;
        
    }

    public void Attack(Vector2 playerPosition)
    {
        Vector2 newPosition = new Vector2(playerPosition.X + _attackRange, playerPosition.Y);
        AttackedStep?.Invoke(AttackType.Start);
        _position = GlobalPosition;

        _movingTween?.Kill();
        _movingTween = CreateTween();

        _movingTween.TweenProperty(this, "global_position", newPosition,
                                   _duration)
                    .From(GlobalPosition)
                    .SetTrans(_transitionType);

        CreateToPlayer();

        return;

        void CreateToPlayer()
        {
            _movingTween?.Kill();
            _movingTween = CreateTween();

            MoveToPlayer(newPosition);
            _movingTween.Finished += Attacked;
        }

        void CreateFromPlayer()
        {
            _movingTween?.Kill();
            _movingTween = CreateTween();

            _movingTween.TweenProperty(this, "global_position", _position, _duration)
                        .From(GlobalPosition)
                        .SetTrans(_transitionType);
            _movingTween.Finished += () => AttackedStep?.Invoke(AttackType.End);
        }

        void Attacked()
        {
            AttackedStep?.Invoke(AttackType.Attacked);
            CreateFromPlayer();
        }
    }

    private void MoveToPlayer(Vector2 playerPosition)
    {
        _movingTween.TweenProperty(this, "global_position", playerPosition, _duration)
                    .From(GlobalPosition)
                    .SetTrans(_transitionType);
    }

    private void OnAttackedStep(AttackType attackType)
    {
        switch(attackType){
            case AttackType.Start:
                break;
            case AttackType.Attacked:
                if(_rayCast2D.GetCollider() is IHealth health)
                    health.TakeDamage(this, _damage);

                break;
            case AttackType.End:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(attackType), attackType, null);
        }
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
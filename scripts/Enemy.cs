using Godot;
using StepToStep.Utils;
using System;
using System.ComponentModel;

namespace StepToStep.scripts;

public partial class Enemy : Node2D, IHealth
{
    private enum Direction
    {
        left,
        right,
        leftUp,
        leftDown,
        rightUp,
        rightDown
    };

    public event Action<AttackType> AttackedStep;

    [Signal] public delegate void DeadEventHandler();

    [Export, Category("Tween")] private float _duration = 1;
    [Export] private Tween.TransitionType _transitionType = Tween.TransitionType.Linear;
    [Export] private float _attackRange = 15;
    [Export] private float _damage = 10;
    [Export] private RayCast2D _rayCast2D;
    [Export] private int _stepCount = 2;

    private Tween _movingTween;
    private Health _health;
    private Vector2[] _steps;
    private int _currentStep;

    public override void _Ready()
    {
        _health = GetNode<Health>("%Health");

        _health.ChangedValue += HealthOnChangedValue;
        AttackedStep += OnAttackedStep;
    }

    public void ReadyToAttack(Vector2 playerPosition)
    {
        if(_steps != null && _steps.Length < _stepCount)
            return;

        _steps = new Vector2[_stepCount + 1];
        Vector2 step = (playerPosition - GlobalPosition) / _stepCount;

        for(int i = 0; i <= _stepCount; i++){
            if(i == 0){
                _steps[i] = GlobalPosition;
                continue;
            }

            _steps[i] = GlobalPosition + step * i;
        }
    }

    public void Attack()
    {
        _currentStep++;
        AttackedStep?.Invoke(AttackType.Start);

        Move(_steps[_currentStep]);

        AttackedStep?.Invoke(AttackType.End);

        return;

        void Move(Vector2 toPosition)
        {
            _movingTween?.Kill();
            _movingTween = CreateTween();

            _movingTween.TweenProperty(this, "global_position", toPosition,
                                       _duration)
                        .From(GlobalPosition)
                        .SetTrans(_transitionType);
            _movingTween.Finished += CheckReturnOnFinished;
            return;

            void CheckReturnOnFinished()
            {
                if(_currentStep != _stepCount)
                    return;
                Attacked();
                Move(_steps[_currentStep = 0]);
            }
        }

        void Attacked()
        {
            AttackedStep?.Invoke(AttackType.Attacked);
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
                // if(_rayCast2D.GetCollider() is IHealth health)
                    // health.TakeDamage(this, _damage);
 
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
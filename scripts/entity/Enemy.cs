using Godot;
using StepToStep.Utils;
using System;
using System.ComponentModel;

namespace StepToStep.Entity;

public partial class Enemy : Node2D, IHealth
{
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
    }

    public void InitialTarget(Vector2 targetPosition)
    {
        if(_steps != null && _steps.Length < _stepCount)
            return;

        _steps = new Vector2[_stepCount + 1];
        Vector2 way = (targetPosition - GlobalPosition) / _stepCount;
        way = new Vector2(way.X + _attackRange, way.Y);
        _rayCast2D.TargetPosition = new Vector2(way.Normalized().X * _attackRange, way.Normalized().Y);

        for(int i = 0; i <= _stepCount; i++){
            if(i == 0){
                _steps[i] = GlobalPosition;
                continue;
            }

            _steps[i] = GlobalPosition + way * i;
        }

        _steps[_stepCount] = new Vector2(_steps[_stepCount].X, _steps[_stepCount].Y);
    }

    public void Attack()
    {
        
        AttackedStep?.Invoke(AttackType.Start);
        _currentStep++;
        Move(_steps[_currentStep]);

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
            
        }
void CheckReturnOnFinished()
            {
                if(_currentStep != _stepCount){
                    AttackedStep?.Invoke(AttackType.End);
                    return;
                }

                Attacked();
                AttackedStep?.Invoke(AttackType.End);
            }

        void Attacked()
        {
            if(_rayCast2D.GetCollider() is not IHealth health)
                return;
            AttackedStep?.Invoke(AttackType.Attacked);
            health.TakeDamage(this, _damage);
        }
    }

    private void MoveToPlayer(Vector2 playerPosition)
    {
        _movingTween.TweenProperty(this, "global_position", playerPosition, _duration)
                    .From(GlobalPosition)
                    .SetTrans(_transitionType);
    }

    public void TakeDamage(Node sender, float damage)
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
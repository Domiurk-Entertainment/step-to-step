using Godot;
using System;
using System.ComponentModel;

namespace StepToStep.scripts;

public partial class Enemy : Node2D, IHealth
{
    public event Action<StepType> ChangeStep;

    [Export, Category("Tween")] private float _duration = 1;
    [Export] private Tween.TransitionType _transitionType = Tween.TransitionType.Linear;
    [Export] private float _attackRange = 15;
    [Export] private RayCast2D _rayCast2D;

    private Tween _movingTween;
    private Vector2 _position;
    private Health _health;

    public override void _Ready()
    {
        _health = GetNode<Health>("%Health");
        ChangeStep += OnChangeStep;
    }

    public bool TryAttack(Vector2 playerPosition)
    {
        ChangeStep?.Invoke(StepType.Start);
        _position = GlobalPosition;

        CreateToPlayer();

        return true;

        void CreateToPlayer()
        {
            _movingTween?.Kill();
            _movingTween = CreateTween();

            _movingTween.TweenProperty(this, "global_position", playerPosition,
                                       _duration)
                        .From(GlobalPosition)
                        .SetTrans(_transitionType);
            _movingTween.Finished += Attacked;
        }

        void CreateFromPlayer()
        {
            _movingTween?.Kill();
            _movingTween = CreateTween();

            _movingTween.TweenProperty(this, "global_position", _position, _duration)
                        .From(GlobalPosition)
                        .SetTrans(_transitionType);
            _movingTween.Finished += () => ChangeStep?.Invoke(StepType.End);
        }

        void Attacked()
        {
            ChangeStep?.Invoke(StepType.Attacked);
            CreateFromPlayer();
        }
    }

    private void MoveToPlayer(Vector2 playerPosition)
    {
        _movingTween.TweenProperty(this, "global_position", playerPosition, _duration)
                    .From(GlobalPosition)
                    .SetTrans(_transitionType);
    }

    private void OnChangeStep(StepType stepType)
    {
        switch(stepType){
            case StepType.Start:
                break;
            case StepType.Attacked:
                // if(rayCast2D.IsColliding()){
                // var target = rayCast2D.GetCollider().GetScript();
                // GD.Print("Target: " + target);
                // }

                break;
            case StepType.End:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stepType), stepType, null);
        }
    }

    public void TakeDamage(object sender, float damage)
    {
        if(damage <= 0)
            return;
        _health.Subtract(sender, damage);
    }
}
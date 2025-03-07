using Godot;
using StepToStep.scripts;
using StepToStep.Utils;
using System;
using Range = System.Range;

namespace StepToStep.Entity;

public partial class BossCave : Node2D
{
    [Signal] public delegate void DeadEventHandler();

    public event Action<AttackType> AttackedStep;

    [Export] private string[] _attackAnimationNames = Array.Empty<string>();

    private AnimationPlayer _animationPlayer;

    private Vector2 _targetPosition;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void InitialTarget(Vector2 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public void Attack()
    {
        if(_attackAnimationNames.Length == 0)
            GD.PrintErr($"{Name} Boss is not animations attack names in property ({nameof(_attackAnimationNames)})");

        _animationPlayer.Play(_attackAnimationNames[GD.RandRange(0, _attackAnimationNames.Length - 1)].ToLower());
    }
}
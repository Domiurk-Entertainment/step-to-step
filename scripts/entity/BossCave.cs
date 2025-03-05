using Godot;
using StepToStep.scripts;
using StepToStep.Utils;
using System;

namespace StepToStep.Entity;

public partial class BossCave : Node2D
{
    [Signal] public delegate void DeadEventHandler();

    public event Action<AttackType> AttackedStep;

    [Export] private string[] _animationNames = Array.Empty<string>();

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
        
    }
}
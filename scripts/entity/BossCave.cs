using Godot;
using StepToStep.Utils;
using System;

namespace StepToStep.Entity;

public partial class BossCave : Enemy
{
    public new event Action<AttackType> AttackedStep;

    [Export] private AnimatedSprite2D _animatedSprite;
    [Export] private string attackAnimationName = "attack";
    [Export] private Node2D _pointToShoot;
    [Export] private PackedScene _projectile;
    private Vector2 _playerPosition;

    public override void _Ready()
    {
        base._Ready();
        _animatedSprite.FrameChanged += AnimatedSpriteOnFrameChanged;
        _animatedSprite.AnimationFinished += AnimatedSpriteOnAnimationFinished;
    }

    private void AnimatedSpriteOnAnimationFinished()
    {
        if(_animatedSprite.GetAnimation() != "attack")
            return;
        AttackedStep?.Invoke(AttackType.End);
    }

    public override void InitialTarget(Vector2 targetPosition)
    {
        _playerPosition = targetPosition;
    }

    private void AnimatedSpriteOnFrameChanged()
    {
        if(_animatedSprite.GetAnimation() != "attack" || _animatedSprite.Frame != 7)
            return;

        var projectile = _projectile.Instantiate<Projectile>();
        _pointToShoot.AddChild(projectile);
        projectile.Position = Vector2.Zero;
        projectile.Shoot(_playerPosition, Damage);
        AttackedStep?.Invoke(AttackType.Attacked);
    }

    public override void Attack()
    {
        AttackedStep?.Invoke(AttackType.Start);
        _animatedSprite.Play("attack");
    }
}
using Godot;

namespace StepToStep.Entity;

public partial class BossCave : Enemy
{
    [Export] private AnimatedSprite2D _animatedSprite;
    [Export] private string attackAnimationName = "attack";
    private Player _player;

    public override void _Ready()
    {
        base._Ready();
        _player = GetTree().CurrentScene.GetNode<Player>("Player");
    }

    public override void Attack()
    {
        
    }

    private void FinishAnimationAttack()
    {
        _animatedSprite.Play(attackAnimationName);
    }

}
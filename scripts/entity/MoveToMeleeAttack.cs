using Godot;
using StepToStep.Entity;
using StepToStep.Utils;

namespace StepToStep.Entity;

public partial class MoveToMeleeAttack : AttackBase {
    [Export] protected float Damage = 10;
    [Export] private float _attackRange = 15;
    [Export] private RayCast2D _rayCast2D;
    [ExportCategory("Tween")]
    [Export] private float _duration = 1;
    [Export] private Tween.TransitionType _transitionType = Tween.TransitionType.Linear;
    [Export] private int _stepCount = 2;

    private string _attackAnimationName = "attack";
    private Tween _movingTween;
    private Vector2[] _steps;
    private int _currentStep;

    private void AttackAnimationOnFinished(StringName animName) {
        if (animName != _attackAnimationName)
            return;
        Self.PlayAnimation("idle");
    }

    public override void InitialTarget(EntityBase target, EntityBase self) {
        base.InitialTarget(target, self);

         Self.AnimatedSprite.AnimationFinished += AnimatedSpriteOnAnimationFinished;

        _steps = new Vector2[_stepCount + 1];
        Vector2 way = (Target.GlobalPosition - Self.GlobalPosition) / _stepCount;
        way = new Vector2(way.X + _attackRange, way.Y);
        _rayCast2D.TargetPosition = new Vector2(way.Normalized().X * _attackRange, way.Normalized().Y);

        for (int i = 0; i <= _stepCount; i++) {
            if (i == 0) {
                _steps[i] = Self.GlobalPosition;
                continue;
            }

            _steps[i] = Self.GlobalPosition + way * i;
        }

        _steps[_stepCount] = new Vector2(_steps[_stepCount].X, _steps[_stepCount].Y);
        return;

        void AnimatedSpriteOnAnimationFinished() {
            AttackAnimationOnFinished(Self.AnimatedSprite.GetAnimation());
        }
    }

    protected void MoveTo(Vector2 toPosition) {
        _movingTween?.Kill();
        _movingTween = CreateTween();

        _movingTween.TweenProperty(Self, "global_position", toPosition,
                _duration)
            .From(Self.GlobalPosition)
            .SetTrans(_transitionType);
        _movingTween.Finished += CheckReturnOnFinished;
        _movingTween.StepFinished += CheckToCollide;

        return;

        void CheckToCollide(long idx) { }

        void CheckReturnOnFinished() {
            if (_currentStep == _stepCount) {
                TryAttack();
            }

            Self.PlayAnimation(_attackAnimationName);

            if (_currentStep != 0)
                EmitSignal(AttackBase.SignalName.ChangeAttackStep, nameof(AttackType.End));
        }
    }

    public override void Start() {
        _currentStep++;
        EmitSignal(AttackBase.SignalName.ChangeAttackStep, nameof(AttackType.Start));
        MoveTo(_steps[_currentStep]);
    }

    private void TryAttack() {

        if (_rayCast2D.GetCollider() is IHealth health) {
            EmitSignal(AttackBase.SignalName.ChangeAttackStep, nameof(AttackType.Attacked));
            health.TakeDamage(this, Damage);
        }

        MoveTo(_steps[_currentStep = 0]);
        EmitSignal(AttackBase.SignalName.ChangeAttackStep, nameof(AttackType.End));
    }
}

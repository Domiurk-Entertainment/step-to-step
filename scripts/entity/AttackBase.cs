using Godot;
using StepToStep.Entity;
using StepToStep.Utils;

namespace StepToStep.Entity;

public abstract partial class AttackBase : Node
{
    [Signal] public delegate void ChangeAttackStepEventHandler(AttackType attackType);

    protected EntityBase Target;
    protected EntityBase Self;

    public virtual void InitialTarget(EntityBase self, EntityBase target)
    {
        Target = target;
        Self = self;
    }

    public virtual void Start() { }
    public virtual void Process() { }
    public virtual void End() { }
}
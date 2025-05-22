using Godot;
using StepToStep.scripts.entity;

namespace StepToStep.Entity;

public abstract partial class EnemyBase : EntityBase, IAttack
{
    [Export] public AttackBase AttackBase { get; private set; }

    public void Attack()
    {
        AttackBase.Start();
    }
    
    public void OnHit(){}
}
using Godot;
using StepToStep.Entity;
namespace StepToStep.scripts.entity;

public partial class ProjectileAttack : AttackBase{
    [Export] private PackedScene _projectileScene;
    [Export] private Node _projectileMuzzle;
    public override void Start() {
        var instance = _projectileScene.Instantiate<Projectile>();
        //todo
    }
}

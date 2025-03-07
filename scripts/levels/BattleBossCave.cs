using Godot;
using StepToStep.Entity;
using StepToStep.InventorySpace;
using StepToStep.Level;
using StepToStep.Systems;
using StepToStep.Utils;
using System;
using System.Linq;

namespace StepToStep.Level;

public partial class BattleBossCave : Node
{
    private const float COOLDOWN_ATTACKS = 2;

    [Export] private Button _playerAttackButton;
    [Export] private Node2D _playerSpawnPoint;
    [Export] private Node2D _enemySpawnPoint;
    [Export] private InventoryInterface _inventory;

    private Player _player;
    private BossCave _enemy;
    private bool _isEnd;

    public override void _Ready()
    {
        BattleConfig config = SceneTransition.GetData(GetTree().CurrentScene.SceneFilePath).As<BattleConfig>();

        _player = config.PlayerPackedScene.Instantiate<Player>();
        _enemy = config.EnemiesPackedScene.Instantiate<BossCave>();

        _player.AttackedStep += PlayerOnAttackedStep;
        _player.Dead += PlayerOnDead;
        _enemy.AttackedStep += EnemyOnAttackedStep;
        _enemy.Dead += EnemyOnDead;

        AddChild(_player);
        AddChild(_enemy);

        _inventory.Initialize(_player.Inventory);
        _player.Inventory.AddItems(config.Items.ToArray());
        _player.GlobalPosition = _playerSpawnPoint.GlobalPosition;
        _enemy.GlobalPosition = _enemySpawnPoint.GlobalPosition;
        _enemy.InitialTarget(_player.GlobalPosition);
    }

    public override void _ExitTree()
    {
        _player.AttackedStep -= PlayerOnAttackedStep;
        _player.Dead -= PlayerOnDead;

        _enemy.AttackedStep -= EnemyOnAttackedStep;
        _enemy.Dead -= EnemyOnDead;
    }

    private void EnemyOnAttackedStep(AttackType step)
    {
        if(_player == null || _enemy == null)
            return;

        switch(step){
            case AttackType.Start:
                break;
            case AttackType.Attacked:
                break;
            case AttackType.End:
                _playerAttackButton.Disabled = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(step), step, null);
        }
    }

    private void PlayerOnAttackedStep(AttackType step)
    {
        switch(step){
            case AttackType.Start:
                break;
            case AttackType.End:
                if(_enemy == null)
                    return;
                _playerAttackButton.Disabled = true;
                SceneTreeTimer timer = GetTree().CreateTimer(COOLDOWN_ATTACKS);
                timer.Timeout += EnemyAttack;
                break;
            case AttackType.Attacked:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(step), step, null);
        }

        return;

        void EnemyAttack()
        {
            if(_isEnd)
                return;
            _enemy.Attack();
        }
    }

    private void EnemyOnDead()
    {
        _enemy.QueueFree();
        UserInterfaceSystem.Instance.Modal.Open("You win", textOneAction: "Back To Map",
                                                oneAction: BackToLastScene);
        _isEnd = true;
        return;

        void BackToLastScene()
        {
            SceneTransition.Instance.LoadLastScene();
        }
    }

    private void PlayerOnDead()
    {
        _player.QueueFree();
        UserInterfaceSystem.Instance.Modal.Open("You Lose", textOneAction: "Back To Map",
                                                oneAction: BackToLastScene);
        _isEnd = true;
        return;

        void BackToLastScene()
        {
            SceneTransition.Instance.LoadLastScene();
        }
    }

    private bool tryingRunOff;

    [Export] private int chanceToRunOff = 4;

    public void TryRunOff()
    {
        if(chanceToRunOff > 0){
            chanceToRunOff /= 100;
            TryRunOff();
            return;
        }

        float value = GD.Randf();

        if(_player.ChanceToRun > value)
            SceneTransition.Instance.LoadLastScene();
    }

    public void PlayerAttack()
    {
        _player.Attack();
    }
}
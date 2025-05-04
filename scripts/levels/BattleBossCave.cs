using Godot;
using StepToStep.Entity;
using StepToStep.InventorySpace;
using StepToStep.Systems;
using StepToStep.Utils;
using System;
using System.Linq;

namespace StepToStep.Level;

public partial class BattleBossCave : Level<BattleConfig>
{
    private const float COOLDOWN_ATTACKS = 2;
    private const float COOLDOWN_SHOW_MENU = 2;

    [Export] private Button _playerAttackButton;
    [Export] private Node2D _playerSpawnPoint;
    [Export] private Node2D _enemySpawnPoint;
    [Export] private InventoryInterface _inventory;

    private Player _player;
    private BossCave _enemy;
    private bool _isEnd;
    [Export] private Camera2D _camera;

    public override void _Ready()
    {
        base._Ready();
        _player = Config.PlayerPackedScene.Instantiate<Player>();
        _enemy = Config.EnemiesPackedScene.Instantiate<BossCave>();

        _player.Hit += PlayerOnHit;
        _player.AttackedStep += PlayerOnAttackedStep;
        _player.Dead += PlayerOnDead;
        _enemy.AttackedStep += EnemyOnAttackedStep;
        _enemy.Dead += EnemyOnDead;

        AddChild(_player);
        AddChild(_enemy);

        _inventory.Initialize(_player.Inventory);
        _player.Inventory.AddItems(Config.Items.ToArray());
        _player.GlobalPosition = _playerSpawnPoint.GlobalPosition;
        _enemy.GlobalPosition = _enemySpawnPoint.GlobalPosition;
        _enemy.InitialTarget(_player.GlobalPosition);
        UserInterfaceSystem.Instance.ShowPauseButton();
        return;

        void PlayerOnHit()
        {
            _camera.Call("start_shake");
        }
    }

    public override void _ExitTree()
    {
        _player.AttackedStep -= PlayerOnAttackedStep;
        _player.Dead -= PlayerOnDead;

        _enemy.AttackedStep -= EnemyOnAttackedStep;
        _enemy.Dead -= EnemyOnDead;
        UserInterfaceSystem.Instance.HidePauseButton();
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
                CreateTimer(COOLDOWN_ATTACKS, EnemyAttack);
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

    private void CreateTimer(float duration, Action callback)
    {
        SceneTreeTimer timer = GetTree().CreateTimer(duration);
        timer.Timeout += callback;
    }

    private void EnemyOnDead()
    {
        CreateTimer(COOLDOWN_SHOW_MENU, ShowModal);
        _isEnd = true;
        return;

        void BackToLastScene()
        {
            SceneTransition.Instance.LoadLastScene();
        }

        void ShowModal()
        {
            _enemy.QueueFree();
            UserInterfaceSystem.Instance.AddModal("You win", textOneAction: "Back To Map",
                                                    oneAction: BackToLastScene);
        }
    }

    private void PlayerOnDead()
    {
        _player.QueueFree();
        UserInterfaceSystem.Instance.AddModal("You Lose", textOneAction: "Back To Map",
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
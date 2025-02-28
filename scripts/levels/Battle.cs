using Godot;
using StepToStep.InventorySpace;
using StepToStep.Level;
using StepToStep.scripts;
using StepToStep.Utilities;
using StepToStep.Utils;
using System;
using System.Linq;

namespace StepToStep.Battle
{
    public partial class Battle : Node2D
    {
        private const float COOLDOWN_ATTACKS = 2;

        [ExportCategory("Global Config Level")]
        [Export] private Button _playerAttackButton;
        [Export] private Node2D _playerSpawnPoint;
        [Export] private Node2D _enemySpawnPoint;
        [Export] private InventoryInterface _inventory;
        private Player _player;
        private Enemy _enemy;

        public override void _Ready()
        {
            BattleConfig config = SceneTransition.Data[GetTree().CurrentScene.SceneFilePath].As<BattleConfig>();

            _player = config.PlayerPackedScene.Instantiate<Player>();
            _enemy = config.EnemiesPackedScene.Instantiate<Enemy>();

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
        }

        public override void _ExitTree()
        {
            _player.AttackedStep -= PlayerOnAttackedStep;
            _player.Dead += PlayerOnDead;

            _enemy.AttackedStep -= EnemyOnAttackedStep;
            _enemy.Dead += EnemyOnDead;
        }

        private void EnemyOnAttackedStep(AttackType step)
        {
            switch(step){
                case AttackType.Start:
                    break;
                case AttackType.Attacked:
                    break;
                case AttackType.End:
                    _playerAttackButton.Show();
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
                    _playerAttackButton.Hide();
                    SceneTreeTimer timer = GetTree().CreateTimer(COOLDOWN_ATTACKS);
                    timer.Timeout += () => _enemy.Attack(_player.GlobalPosition);
                    break;
                case AttackType.Attacked:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(step), step, null);
            }
        }

        private void EnemyOnDead()
        {
            _enemy.QueueFree();
        }

        private void PlayerOnDead()
        {
            _player.QueueFree();
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
                SceneTransition.Instance.ChangeScene(SceneTransition.ScenesHistory.Pop());
        }

        public void PlayerAttack()
        {
            _player.Attack();
        }
    }
}
using Godot;
using StepToStep.Entity;
using StepToStep.InventorySystem;
using StepToStep.Levels;
using StepToStep.Systems;
using StepToStep.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StepToStep.Battle
{
    public partial class Battle : Level<BattleConfig>
    {
        [Signal] public delegate void PlayerStartTurnEventHandler();

        [Signal] public delegate void PlayerEndTurnEventHandler();

        private const float COOLDOWN_ATTACKS = 2;
        private const float COOLDOWN_SHOW_MENU = 1;

        [ExportCategory("Global Config Level")]
        [Export] private Node2D _playerSpawnPoint;
        [Export] private Node2D _enemySpawnPoint;
        [Export] private InventoryInterface _inventory;

        private Player _player;
        private Enemy _enemy;
        private bool _isEnd;

        [Export] private Camera2D _camera;

        public override void _Ready()
        {
            base._Ready();

            _player = Config.PlayerPackedScene.Instantiate<Player>();
            _enemy = Config.EnemiesPackedScene.Instantiate<Enemy>();

            _player.Hit += PlayerOnHit;
            _player.AttackedStep += PlayerOnAttackedStep;
            _player.Dead += PlayerOnDead;

            _enemy.Connect(EntityBase.SignalName.Dead, Callable.From(EnemyOnDead));
            _enemy.Dead += EnemyOnDead;

            AddChild(_player);
            AddChild(_enemy);

            _inventory.Initialize(_player.Inventory);

            _player.Inventory.AddItems(Config.Items.ToArray());
            _player.GlobalPosition = _playerSpawnPoint.GlobalPosition;

            _enemy.GlobalPosition = _enemySpawnPoint.GlobalPosition;
            _enemy.InitialTarget(_player.GlobalPosition);

            UserInterfaceSystem.Instance.ShowPauseButton();

            _enemy.AttackBase.InitialTarget(_enemy, _player);
            _enemy.AttackBase.Connect(AttackBase.SignalName.ChangeAttackStep,
                                      Callable.From<string>(EnemyChangeAttackState));

            _turns.Enqueue(_player);
            _turns.Enqueue(_enemy);
            _turns.Enqueue(_player);
            _turns.Enqueue(_enemy);
            _turns.Enqueue(_player);
            _turns.Enqueue(_enemy);
            _turns.Enqueue(_player);
            _turns.Enqueue(_enemy);

            EmitSignal(CheckNextTurn<Player>() ? SignalName.NextTurnPlayer : SignalName.NextTurnEnemy);

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

            UserInterfaceSystem.Instance.ShowUserInterfaces(UserInterfacesType.PausePanel |
                                                            UserInterfacesType.CoinSystem);
        }

        private void EnemyAttack()
        {
            if(_isEnd)
                return;
            _enemy.Attack();
        }

        private void CreateTimer(float duration, Action callback)
        {
            SceneTreeTimer timer = GetTree().CreateTimer(duration);
            timer.Timeout += callback;
        }

        private void EnemyOnDead()
        {
            CreateTimer(COOLDOWN_SHOW_MENU, EndLevel);
            _isEnd = true;
            return;

            void BackToLastScene()
            {
                SceneTransition.Instance.LoadLastScene();
            }

            void EndLevel()
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

        public void TryRunOff()
        {
            float value = GD.Randf();

            if(_player.ChanceToRun > value)
                SceneTransition.Instance.LoadLastScene();
            else{
                EmitSignal(SignalName.PlayerStartTurn);
                CreateTimer(COOLDOWN_ATTACKS, AttackEnemy);

                void AttackEnemy()
                {
                    EnemyAttack();
                }
            }
        }

        public void PlayerAttack()
        {
            _player.Attack();
        }

        private const float CHANCE_TURN_PLAYER = 0.1f;

        [Signal] public delegate void NextTurnEnemyEventHandler();
        [Signal] public delegate void NextTurnPlayerEventHandler();

        private readonly Queue<IAttack> _turns = new();
        private IAttack _currentTurn;

        private void PlayerOnAttackedStep(AttackType attack)
        {

            switch(attack){
                case AttackType.Start:
                    AddRandomTurn();
                    break;
                case AttackType.Attacked:
                    break;
                case AttackType.End:
                    _currentTurn = null;
                    EndTurn();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(attack), attack, null);
            }
        }

        private void EnemyChangeAttackState(string stateName)
        {
            switch(stateName.ParseToEnum<AttackType>()){
                case AttackType.End:
                    _currentTurn = null;
                    if(_player.Inventory.Balls.Count == 0 && _player.Inventory.CurrentBall == null)
                        UserInterfaceSystem.Instance.AddModal("You lose!!!", textOneAction: "Back To Map",
                                                              oneAction: SceneTransition.Instance.LoadLastScene);
                    EndTurn();
                    break;
                case AttackType.Start:
                    AddRandomTurn();
                    break;

                case AttackType.Attacked:
                    _camera.Call("start_shake");

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void EndTurn()
        {

            if(CheckNextTurn<EnemyBase>(force: true)){
                EmitSignal(SignalName.NextTurnEnemy);
                StartEnemyAttack();
            }
            else{
                EmitSignal(SignalName.NextTurnPlayer);
            }
        }

        private void AddRandomTurn()
        {
            _turns.Enqueue(GD.Randf() > CHANCE_TURN_PLAYER ? _player : _enemy);
        }

        public void StartAttack()
        {
            if(_currentTurn == null)
                GD.Print($"{nameof(Battle)}: Current turn is null");
            _currentTurn?.Attack();
        }

        public void StartEnemyAttack()
            => GetTree()
               .CreateTimer(COOLDOWN_ATTACKS)
               .Connect(SceneTreeTimer.SignalName.Timeout, Callable.From(StartAttack));

        private bool CheckNextTurn<TAttack>(bool force = false) where TAttack : IAttack
        {
            bool result = _turns.Peek() is TAttack;
            if(result || force)
                _currentTurn = _turns.Dequeue();
            return result;
        }
    }
}
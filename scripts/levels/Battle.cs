using Godot;
using StepToStep.Entity;
using StepToStep.InventorySystem;
using StepToStep.Levels;
using StepToStep.Systems;
using StepToStep.Utils;
using System;
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

			_enemy.AttackedStep += EnemyOnAttackedStep;
			_enemy.Connect(Enemy.SignalName.Dead, Callable.From(EnemyOnDead));
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
			UserInterfaceSystem.Instance.ShowUserInterfaces(UserInterfacesType.PausePanel |
															UserInterfacesType.CoinSystem);
		}

		private void EnemyOnAttackedStep(AttackType step)
		{
			if(_player == null || _enemy == null)
				return;

			switch(step){
				case AttackType.Start:
					break;
				case AttackType.Attacked:
					_camera.Call("start_shake");
					break;
				case AttackType.End:
					if(_player.Inventory.Balls.Count > 0 || _player.Inventory.CurrentBall != null)
						EmitSignal(SignalName.PlayerEndTurn);
					else
						UserInterfaceSystem.Instance.AddModal("You lose!!!", textOneAction: "Back To Map",
															  oneAction: SceneTransition.Instance.LoadLastScene);
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
					CreateTimer(COOLDOWN_ATTACKS, EnemyAttack);
					break;
				case AttackType.Attacked:
					EmitSignal(SignalName.PlayerStartTurn);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(step), step, null);
			}
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
	}
}

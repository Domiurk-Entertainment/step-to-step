using Godot;
using StepToStep.Level;
using StepToStep.scripts;
using StepToStep.Utils;
using System.Linq;

namespace StepToStep.Battle
{
    public partial class Battle : Node2D
    {
        [ExportCategory("Global Config Level")]
        [Export] private Node2D playerSpawnPoint;
        [Export] private Node2D enemySpawnPoint;

        private bool playerTurn = true;
        private Player _player;

        private Enemy _enemy;

        public override void _Ready()
        {
            BattleConfig config = SceneTransition.Data[GetTree().CurrentScene.SceneFilePath].As<BattleConfig>();
            _player = config.PlayerPackedScene.Instantiate<Player>();
            _enemy = config.EnemiesPackedScene.Instantiate<Enemy>();

            AddChild(_player);
            AddChild(_enemy);

            _player.GlobalPosition = playerSpawnPoint.GlobalPosition;
            _enemy.GlobalPosition = enemySpawnPoint.GlobalPosition;
            
            _player.Inventory.AddItems(config.Items.ToArray());
        }

        private bool tryingRunOff;
        [Export] private int chanceToRunOff = 4;

        public async void TryRunOff()
        {
            if(tryingRunOff)
                return;

            tryingRunOff = true;

            for(int i = chanceToRunOff; i > 0; i--){
                SceneTreeTimer timer = GetTree().CreateTimer(1);
                await ToSignal(timer, "timeout");
                GD.Print(i + "...");
            }

            SceneTreeTimer timerAfter = GetTree().CreateTimer(1);
            await ToSignal(timerAfter, "timeout");
            GD.Print("Ви спробували втікти але натрапили на пастку");
            tryingRunOff = false;
        }

        public void PlayerAttack()
        {
            _player.Attack();
        }
    }
}
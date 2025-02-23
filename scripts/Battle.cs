using Godot;
using StepToStep.Level;
using StepToStep.scripts;

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

        public void InitializeLevel(BattleConfig config)
        {
            GD.Print(GetNode("/root/SceneTransition")
                     .Get("data")
                     .AsGodotDictionary()[Name]);

            if(_player != null || _enemy != null)
                return;
            // _player = GD.Load<Player>(config.PlayerPackedScene.ResourcePath);
            // _enemy = GD.Load<Enemy>(config.EnemiesPackedScene.ResourcePath);
            _player = config.PlayerPackedScene.Instantiate<Player>();
            _enemy = config.EnemiesPackedScene.Instantiate<Enemy>();

            _player.GlobalPosition = playerSpawnPoint.GlobalPosition;
            _enemy.GlobalPosition = enemySpawnPoint.GlobalPosition;
            GD.Print($"{_player.Name}:{_player.GlobalPosition}");
            GD.Print($"{_enemy.Name}:{_enemy.GlobalPosition}");
        }

        public void TryRunOff()
        {
            GetNode("/root/SceneTransition").Call("change_scene", "res://scenes/map_sprite.tscn");
        }

        public void PlayerAttack() { }
    }
}
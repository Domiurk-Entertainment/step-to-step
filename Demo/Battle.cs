using Godot;
using StepToStep.scripts;
using System;

public partial class Battle : Node2D
{
    [ExportCategory("Entities")]
    [Export(PropertyHint.File)] private PackedScene _playerPackedScene;
    [Export(PropertyHint.File)] private PackedScene _enemiesPackedScene;
    [Export] private int enemyCount;

    [ExportCategory("Config Level")]
    [Export] private Node2D playerSpawnPoint;
    [Export] private Node2D enemySpawnPoint;

    [ExportCategory("UI")]
    [Export] private Button attackButton;
    [Export] private Button runOffButton;

    private bool playerTurn = true;
    private Player _player;
    private Enemy _enemy;

    public void InitializeLevel()
    {
        _player = _playerPackedScene.Instantiate<Player>();
        _enemy = _enemiesPackedScene.Instantiate<Enemy>();
        _player.GlobalPosition = playerSpawnPoint.GlobalPosition;
        _enemy.GlobalPosition = enemySpawnPoint.GlobalPosition;
    }

    public void TryRunOff()
    {
        GetNode("/root/SceneTransition").Call("change_scene", "res://scenes/map_sprite.tscn");
    }

    public void PlayerAttack() { }
}
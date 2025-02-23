using Godot;

namespace StepToStep.Level;

[GlobalClass]
public partial class BattleConfig : Resource
{
    public PackedScene PlayerPackedScene => _playerPackedScene;
    public PackedScene EnemiesPackedScene => _enemiesPackedScene;
    [Export] private PackedScene _playerPackedScene;
    [Export] private PackedScene _enemiesPackedScene;
    [Export] private int enemyCount;

}
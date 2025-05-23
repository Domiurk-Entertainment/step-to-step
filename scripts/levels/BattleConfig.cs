using Godot;
using StepToStep.InventorySystem;
using System.Collections.Generic;

namespace StepToStep.Levels;

[GlobalClass]
public partial class BattleConfig : LevelConfig
{
    public IReadOnlyCollection<Item> Items => _items;
    [Export] public PackedScene PlayerPackedScene { get; private set; }
    [Export] public PackedScene EnemiesPackedScene { get; private set; }
    [Export] private Item[] _items;
}
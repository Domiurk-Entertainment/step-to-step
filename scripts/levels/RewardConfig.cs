using Godot;
using StepToStep.InventorySystem;

namespace StepToStep.Levels;

[GlobalClass]
public partial class RewardConfig : LevelConfig {
    [Export] public Item ItemReward { get; private set; }
}

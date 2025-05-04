using Godot;
using StepToStep.Entity;
using StepToStep.Systems;

namespace StepToStep.Level;

public partial class Level<TConfig> : Node where TConfig : LevelConfig
{
    protected TConfig Config;

    public override void _Ready()
    {
        if(typeof(TConfig) != typeof(LevelConfig)){
            Config = SceneTransition.GetData(GetTree().CurrentScene.SceneFilePath).As<LevelConfig>() as TConfig;
        }
        else
            GD.Print($"Level: Config is LEVEL CONFIG");

        if(Config != null)
            GD.Print($"Config: {Config.ResourcePath}");
    }
}
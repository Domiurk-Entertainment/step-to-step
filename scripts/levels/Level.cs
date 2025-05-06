using Godot;
using StepToStep.Entity;
using StepToStep.Systems;

namespace StepToStep.Levels;

public partial class Level<TConfig> : Node where TConfig : LevelConfig
{
    protected TConfig Config;

    public override void _Ready()
    {
        if(typeof(TConfig) != typeof(LevelConfig))
            Config = SceneTransition.GetData(GetTree().CurrentScene.SceneFilePath).As<LevelConfig>() as TConfig;
    }

    public void PlaySound(string soundName)
    {
        SoundSystem.Instance.TryPlay(soundName);
    }
}
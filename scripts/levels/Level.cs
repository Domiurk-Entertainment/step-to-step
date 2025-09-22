using Godot;
using StepToStep.Systems;

namespace StepToStep.Levels;

public partial class Level<TConfig> : Node where TConfig : LevelConfig {
    protected TConfig Config;

    public override void _Ready() {
        if (typeof(TConfig) != typeof(LevelConfig))
            Config = (TConfig)SaveSystem.TemporaryData;
    }

    public override void _ExitTree() {
        if (Config == null)
            return;
        SaveSystem.Instance.Set(SectionType.Level, Config.CurrentSceneKey, Config.CurrentSceneValue);
    }

    public void PlaySound(string soundName) {
        SoundSystem.Instance.TryPlay(soundName);
    }
}

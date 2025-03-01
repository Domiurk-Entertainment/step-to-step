using Godot;
using Godot.Collections;
using FileAccess = Godot.FileAccess;

namespace StepToStep.Systems;

public partial class SaveSystem : Node
{
    public static SaveSystem Instance;

    [Export] private string pathConfig = "user://";
    [Export] private string fileName = "config.cfg";
    [Export] private bool canSave = true;

    public override void _Ready()
    {
        if(Instance != null && Instance != this){
            GD.PrintErr($"{Name} is duplicate. {Instance.Name} is original.");
            QueueFree();
            return;
        }
        else{
            Instance = this;
        }

        if(!FileAccess.FileExists(GetConfigPath()))
            new ConfigFile().Save(GetConfigPath());
    }

    public void SaveData(TypeConfiguration typeConfiguration, string key, Variant data)
    {
        if(!canSave)
            return;
        
        ConfigFile config = GetConfigFile();

        config.SetValue(typeConfiguration.ToString(), key, data);
        config.Save(GetConfigPath());
    }

    public Variant LoadData(TypeConfiguration typeConfiguration, string key, Variant defaultData = new Variant())
    {
        ConfigFile config = GetConfigFile();
        return config.GetValue(typeConfiguration.ToString(), key, defaultData);
    }

    public int LoadIntData(TypeConfiguration typeConfiguration, string key, int defaultData = 0)
        => LoadData(typeConfiguration, key, defaultData).AsInt32();

    public string LoadStringData(TypeConfiguration typeConfiguration, string key, string defaultData = "")
        => LoadData(typeConfiguration, key, defaultData).AsString();

    public float LoadFloatData(TypeConfiguration typeConfiguration, string key, float defaultData = 0)
        => LoadData(typeConfiguration, key, defaultData).As<float>();

    public bool LoadBoolData(TypeConfiguration typeConfiguration, string key, bool defaultData = false)
        => LoadData(typeConfiguration, key, defaultData).AsBool();

    public Array LoadArrayData(TypeConfiguration typeConfiguration, string key, Array defaultData = default)
        => LoadData(typeConfiguration, key, defaultData).AsGodotArray();

    public Dictionary LoadDictionaryData(TypeConfiguration typeConfiguration,
                                         string key,
                                         Dictionary defaultData = default)
        => LoadData(typeConfiguration, key, defaultData).AsGodotDictionary();

    private ConfigFile GetConfigFile()
    {
        var result = new ConfigFile();

        Error error = result.Load(GetConfigPath());

        if(error == Error.FileNotFound){
            result.Save(GetConfigPath());
        }

        if(error != Error.Ok){
            GD.PrintErr(error);
        }

        return result;
    }

    private string GetConfigPath() => pathConfig.PathJoin(fileName);
}

public enum TypeConfiguration
{
    Player,
    Level,
    Setting
}
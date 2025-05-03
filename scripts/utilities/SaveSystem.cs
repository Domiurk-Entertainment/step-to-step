using Godot;
using Godot.Collections;
using System;
using System.IO;
using System.Linq;
using FileAccess = Godot.FileAccess;

namespace StepToStep.Systems;

public partial class SaveSystem : Node
{
    public static SaveSystem Instance { get; private set; }

    [Export] private string pathConfig = "user://";
    [Export] private string fileName = "config.cfg";
    [Export] private bool canSave = true;

    private Dictionary<SectionType, Dictionary<string, Variant>>
        dataToSave = new();

    public override void _Ready()
    {
        if(Instance != null && Instance != this){
            GD.PrintErr($"{Name} is duplicate. {Instance.Name} is original.");
            QueueFree();
            return;
        }

        Instance = this;
    }

    public void RemoveAllData()
    {
        if(!ExistConfig() || dataToSave.Count == 0)
            return;

        foreach(SectionType sectionType in Enum.GetValues<SectionType>()){
            dataToSave[sectionType].Clear();
        }
    }

    public void RemoveData(SectionType sectionType, string key)
    {
        if(!dataToSave[sectionType].ContainsKey(key))
            return;
        dataToSave[sectionType].Remove(key);
    }

    public void SaveDictionary()
    {
        string stringify = Json.Stringify(dataToSave, "\t");
        FileAccess file = FileAccess.Open(GetFullPath(), FileAccess.ModeFlags.Write);
        file.StoreString(stringify);
        file.Close();
    }

    public Variant Get(SectionType sectionType, string key, Variant defaultValue)
    {
        if(!dataToSave.TryGetValue(sectionType, out Dictionary<string, Variant> section))
            return defaultValue;

        return section.ContainsKey(key) ? dataToSave[sectionType][key] : defaultValue;
    }

    public void Set(SectionType sectionType, string key, Variant value)
    {
        if(!dataToSave.ContainsKey(sectionType))
            dataToSave.Add(sectionType, new Dictionary<string, Variant>());

        if(dataToSave[sectionType].ContainsKey(key))
            dataToSave[sectionType][key] = value;
        else
            dataToSave[sectionType].Add(key, value);
    }

    public void LoadData()
    {
        if(!ExistConfig()){
            return;
        }

        FileAccess file = FileAccess.Open(GetFullPath(), FileAccess.ModeFlags.Read);

        string jsonString = file.GetAsText();

        file.Close();

        var json = new Json();
        Error error = json.Parse(jsonString);

        if(error != Error.Ok){
            GD.PrintErr(error.ToString());
            return;
        }

        Dictionary<string, Dictionary<string, Variant>> dataDictionary =
            json.Data.AsGodotDictionary<string, Dictionary<string, Variant>>();
        SectionType[] sections = Enum.GetValues<SectionType>();

        foreach(string key in dataDictionary.Keys)
            dataToSave.Add(sections[int.Parse(key)], dataDictionary[key]);
    }

    private bool ExistConfig()
        => FileAccess.FileExists(GetFullPath());

    public bool ExistSaves() => ExistConfig() && dataToSave.Keys.All(ExistSave);

    public bool ExistSave(SectionType sectionType)
        => ExistSaves() && dataToSave.ContainsKey(sectionType) && dataToSave[sectionType].Count > 0;

    private string GetFullPath()
        => Path.Combine(GetFolderPath(), fileName);

    private string GetFolderPath()
        => ProjectSettings.GlobalizePath(pathConfig);

    public override void _EnterTree()
        => LoadData();

    public override void _Notification(int what)
    {
        if(what != NotificationWMCloseRequest)
            return;
        SaveDictionary();
    }
}

public enum SectionType
{
    Player,
    Level,
    Setting
}
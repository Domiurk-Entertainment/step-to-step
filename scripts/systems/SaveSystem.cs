using Godot;
using Godot.Collections;
using System;
using System.IO;
using System.Linq;
using CollectionExtensions = System.Collections.Generic.CollectionExtensions;
using FileAccess = Godot.FileAccess;

namespace StepToStep.Systems;

public partial class SaveSystem : System<SaveSystem>
{
    [Export] private string _pathConfig = "user://";
    [Export] private string _fileName = "config.cfg";

    public static Variant TemporaryData;
    private Dictionary<SectionType, Dictionary<string, Variant>>
        _dataToSave = new();

    public override void _Ready()
    {
        base._Ready();
        LoadData();
    }

    public void RemoveAllData()
    {
        if(!ExistConfig() || _dataToSave.Count == 0)
            return;

        foreach(SectionType sectionType in Enum.GetValues<SectionType>()){
            if(!_dataToSave.ContainsKey(sectionType)){
                _dataToSave.Add(sectionType, new());
                continue;
            }

            _dataToSave[sectionType].Clear();
        }
    }

    public void RemoveData(SectionType sectionType, string key)
    {
        if(!_dataToSave[sectionType].ContainsKey(key))
            return;
        _dataToSave[sectionType].Remove(key);
    }

    public void SaveToFile(Dictionary<SectionType, Dictionary<string, Variant>> dictionaryToFile)
    {
        string stringify = Json.Stringify(dictionaryToFile, "\t");
        FileAccess file = FileAccess.Open(GetFullPath(), FileAccess.ModeFlags.Write);
        file.StoreString(stringify);
        file.Close();
    }

    public Variant Get(SectionType sectionType, string key, Variant defaultValue)
    {
        if(_dataToSave == null)
            GD.Print("Data is null");
        return !_dataToSave!.TryGetValue(sectionType, out Dictionary<string, Variant> section)
                   ? defaultValue
                   : CollectionExtensions.GetValueOrDefault(section, key, defaultValue);
    }

    public void Set(SectionType sectionType, string key, Variant value)
    {
        if(!_dataToSave.ContainsKey(sectionType))
            _dataToSave.Add(sectionType, new());

        if(_dataToSave[sectionType].ContainsKey(key))
            _dataToSave[sectionType][key] = value;
        else
            _dataToSave[sectionType].Add(key, value);
    }

    public void LoadData()
    {
        if(!ExistConfig()){
            RemoveAllData();
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
            _dataToSave.Add(sections[int.Parse(key)], dataDictionary[key]);
    }

    private bool ExistConfig()
        => FileAccess.FileExists(GetFullPath());

    public bool ExistSaves() => ExistConfig() && _dataToSave.Keys.All(ExistSave);

    public bool ExistSave(SectionType sectionType)
        => _dataToSave.ContainsKey(sectionType) && _dataToSave[sectionType].Count > 0;

    private string GetFullPath()
        => Path.Combine(GetFolderPath(), _fileName);

    private string GetFolderPath()
        => ProjectSettings.GlobalizePath(_pathConfig);

    public override void _Notification(int what) {
        return;
        if(what == NotificationWMCloseRequest)
            SaveToFile(_dataToSave);
    }
}

public enum SectionType
{
    Player,
    Level,
    Setting
}
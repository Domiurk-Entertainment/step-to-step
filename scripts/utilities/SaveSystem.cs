using Godot;
using Godot.Collections;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Array = Godot.Collections.Array;
using FileAccess = Godot.FileAccess;

namespace StepToStep.Systems;

//TODO перевірити на логіку відкрииття
//TODO В ентер він запускає лоад в якому перевіряє чи файл присутній за шляхом якщо ні то створює файл 
//TODO при додаванні ключа Якщо немає секції він додає якщо немає ключа теж додає, якщо все є то заміняє значення
//TODO перевірити парсинг файлу в словник

public partial class SaveSystem : Node
{
    public static SaveSystem Instance;

    [Export] private string pathConfig = "user://";
    [Export] private string fileName = "config.cfg";
    [Export] private bool canSave = true;

    private Dictionary<TypeConfiguration, Dictionary<string, Variant>> _dataToSave =
        new();

    // private readonly List<SaveElement> _saveElements = new();

    public override void _Ready()
    {
        if(Instance != null && Instance != this){
            GD.PrintErr($"{Name} is duplicate. {Instance.Name} is original.");
            QueueFree();
            return;
        }

        Instance = this;
    }

    public void AddData(TypeConfiguration typeConfiguration, string key, Variant value)
    {
        // _saveElements.Add(new SaveElement(typeConfiguration.ToString(), key, value));
        _dataToSave[typeConfiguration].Add(key, value);
    }

    public void RemoveAllDataFromDictionaryWithType(TypeConfiguration typeConfiguration)
    {
        _dataToSave[typeConfiguration].Clear();
    }

    public void RemoveAllDataFromDictionary()
    {
        foreach(TypeConfiguration key in _dataToSave.Keys){
            _dataToSave[key].Clear();
        }
    }

    public void RemoveData(TypeConfiguration typeConfiguration, string key)
    {
        Dictionary<string, Variant> data = _dataToSave[typeConfiguration];
        if(!data.ContainsKey(key))
            return;
        _dataToSave[typeConfiguration].Remove(key);
    }

    public void SaveDictionary()
    {
        string test = Json.Stringify(_dataToSave, "\t");

        // foreach(KeyValuePair<TypeConfiguration, Godot.Collections.Dictionary<string, Variant>> sectionPair in _dataToSave){
        // foreach(KeyValuePair<string, Variant> pair in sectionPair.Value){

        // }
        // }

        // var convertedData = new Godot.Collections.Dictionary<string, Godot.Collections.Dictionary<string, Variant>>();

        // foreach(KeyValuePair<TypeConfiguration,
        // Godot.Collections.Dictionary<string, Variant>> kvp in _dataToSave){
        // convertedData[kvp.Key.ToString()] = kvp.Value;
        // }

        // string jsonString = JsonSerializer.Serialize(convertedData, new JsonSerializerOptions{ WriteIndented = true });
        FileAccess file = FileAccess.Open(GetFullPath(), FileAccess.ModeFlags.Write);
        file.StoreString(test);
        file.Close();

        // string jsonString = JsonSerializer.Serialize(_dataToSave, new JsonSerializerOptions { WriteIndented = true });
        // FileAccess file = FileAccess.Open(GetFullPath(), FileAccess.ModeFlags.Write);
        // file.StoreString(jsonString);
        // file.Close();
    }

    public Variant Get(TypeConfiguration typeConfiguration, string key, Variant defaultValue)
    {
        if(!_dataToSave.ContainsKey(typeConfiguration)){
            GD.PrintErr($"Section {typeConfiguration} not found..");
            return defaultValue;
        }

        if(!_dataToSave[typeConfiguration].ContainsKey(key)){
            GD.PrintErr($"NOT Key {key} .");
            return defaultValue;
        }

        return !_dataToSave[typeConfiguration].ContainsKey(key) ? defaultValue : _dataToSave[typeConfiguration][key];
    }

    public void Set(TypeConfiguration typeConfiguration, string key, Variant value)
    {
        if(!_dataToSave[typeConfiguration].ContainsKey(key)){
            _dataToSave[typeConfiguration].Add(key, value);
            GD.Print($"Adding key {key} to {typeConfiguration}");
            return;
        }

        _dataToSave[typeConfiguration][key] = value;
    }

    public void LoadData()
    {
        if(!ExistConfig()){
            foreach(TypeConfiguration configuration in Enum.GetValues<TypeConfiguration>()){
                _dataToSave.Add(configuration, new Dictionary<string, Variant>());
            }

            return;
        }

        FileAccess file = FileAccess.Open(GetFullPath(), FileAccess.ModeFlags.Read);

        string jsonString = file.GetAsText();

        file.Close();
        _dataToSave = Json.ParseString(jsonString).AsGodotDictionary<TypeConfiguration, Dictionary<string, Variant>>();
        // var loadedData =
        // JsonSerializer
        // .Deserialize<
        // System.Collections.Generic.Dictionary<string,
        // System.Collections.Generic.Dictionary<string, object>>>(jsonString);
        // if(loadedData == null)
        // return;

        // _dataToSave.Clear();

        // foreach(var kvp in loadedData){
        // if(!Enum.TryParse(kvp.Key, out TypeConfiguration keyEnum))
        // continue;

        // var innerDict = new Godot.Collections.Dictionary<string, Variant>();

        // foreach(var innerKvp in kvp.Value){
        // innerDict[innerKvp.Key] = (Variant)(innerKvp.Value);
        // }

        // _dataToSave[keyEnum] = innerDict;
        // }

        // if (!FileAccess.FileExists(GetFullPath()))
        // {
        // GD.Print("Save file not found, creating new one...");
        // SaveData(); // Створюємо файл, якщо його немає
        // return;
        // }

        // FileAccess file = FileAccess.Open(GetFullPath(), FileAccess.ModeFlags.Read);
        // string jsonString = file.GetAsText();
        // file.Close();

        // _dataToSave = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString) ?? new Dictionary<string, object>();
    }

    private bool ExistConfig()
        => FileAccess.FileExists(GetFullPath());

    public void RemoveAllData()
    {
        ConfigFile config = GetConfigFile();
        config.Clear();
        config.Save(GetFullPath());
    }

    public void SaveData(TypeConfiguration typeConfiguration, string key, Variant data)
    {
        if(!canSave)
            return;

        ConfigFile config = GetConfigFile();
        config.SetValue(typeConfiguration.ToString(), key, data);
        config.Save(GetFullPath());
    }

    public Variant LoadData(TypeConfiguration typeConfiguration, string key, Variant defaultData = new Variant())
    {
        ConfigFile config = GetConfigFile();
        return config.GetValue(typeConfiguration.ToString(), key, defaultData);
    }

    public int LoadIntData(TypeConfiguration typeConfiguration, string key, int defaultData = 0)
        => LoadData(typeConfiguration, key, defaultData).AsInt32();

    /*
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
        */

    private ConfigFile GetConfigFile()
    {
        var result = new ConfigFile();

        Error error = result.Load(GetFullPath());

        if(error == Error.FileNotFound){
            error = result.Save(GetFullPath());
        }

        if(error != Error.Ok){
            GD.PrintErr(error);
        }

        return result;
    }

    private string GetFullPath()
    {
        return Path.Combine(GetFolderPath(), fileName);
    }

    private string GetFolderPath()
    {
        return ProjectSettings.GlobalizePath(pathConfig);
    }

    public override void _EnterTree()
    {
        LoadData();
        GD.Print($"Save System: Loaded {_dataToSave[TypeConfiguration.Level].Count} counts saves");
    }

    public override void _Notification(int what)
    {
        if(what != NotificationWMCloseRequest)
            return;
        SaveDictionary();
        GD.Print($"{Name}: Saved in file {GetFullPath()}");
    }
}

public class SaveElement
{
    public string Section { get; }
    public string Key { get; }
    [field: JsonConverter(typeof(object))]
    public Variant Value { get; }

    public SaveElement(string section, string key, Variant value)
    {
        Section = section;
        Key = key;
        Value = value;
    }
}

public enum TypeConfiguration
{
    Player,
    Level,
    Setting
}
using Godot;
using Godot.Collections;
using System;
using System.IO;
using Array = Godot.Collections.Array;

namespace StepToStep.Systems;

public partial class SaveSystem : Node
{
	public static SaveSystem Instance;

	[Export] private string pathConfig = "user://";
	[Export] private string fileName = "config.cfg";
	[Export] private bool canSave = true;

	private Dictionary<string, Dictionary<string, Variant>> _dataToSave = new();

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

		if(_dataToSave.Count == 0)
			foreach(TypeConfiguration typeConfiguration in Enum.GetValues<TypeConfiguration>())
				_dataToSave.Add(typeConfiguration.ToString(), new Dictionary<string, Variant>());

		if(!ExistConfig())
			new ConfigFile().Save(GetFullPath());
		GD.Print(ExistConfig());
	}

	public void RemoveDataFromKey(TypeConfiguration typeConfiguration, string key)
	{
		ConfigFile config = GetConfigFile();
		ConfigFile newConfig = new ConfigFile();

		foreach(string section in config.GetSections()){
			foreach(string sectionKey in config.GetSectionKeys(section)){
				if(section == typeConfiguration.ToString() && key == sectionKey){
					continue;
				}

				Variant data = config.GetValue(section, sectionKey);
				newConfig.SetValue(section, sectionKey, data);
			}
		}

		newConfig.Save(GetFullPath());
	}

	private bool ExistConfig()
		=> !File.Exists(GetFullPath());

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
}

public enum TypeConfiguration
{
	Player,
	Level,
	Setting
}

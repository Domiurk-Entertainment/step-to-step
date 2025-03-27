using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileAccess = Godot.FileAccess;

namespace StepToStep.Systems;

//TODO В ентер він запускає лоад в якому перевіряє чи файл присутній за шляхом якщо ні то створює файл 
//TODO при додаванні ключа Якщо немає секції він додає якщо немає ключа теж додає, якщо все є то заміняє значення
//TODO перевірити парсинг файлу в словник

public partial class SaveSystem : Node
{
	public static SaveSystem Instance;

	[Export] private string pathConfig = "user://";
	[Export] private string fileName = "config.cfg";
	[Export] private bool canSave = true;

	private Godot.Collections.Dictionary<SectionType, Godot.Collections.Dictionary<string, Variant>>
		_dataToSave = new();

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
		_dataToSave.Clear();
	}

	public void RemoveData(SectionType sectionType, string key)
	{
		if(!_dataToSave[sectionType].ContainsKey(key))
			return;
		_dataToSave[sectionType].Remove(key);
	}

	public void SaveDictionary()
	{
		string stringify = Json.Stringify(_dataToSave, "\t");
		FileAccess file = FileAccess.Open(GetFullPath(), FileAccess.ModeFlags.Write);
		file.StoreString(stringify);
		file.Close();
	}

	public Variant Get(SectionType sectionType, string key, Variant defaultValue)
	{
		if(!_dataToSave.TryGetValue(sectionType, out Godot.Collections.Dictionary<string, Variant> section))
			return defaultValue;

		return section.ContainsKey(key) ? _dataToSave[sectionType][key] : defaultValue;
	}

	public void Set(SectionType sectionType, string key, Variant value)
	{
		if(!_dataToSave.ContainsKey(sectionType)){
			_dataToSave.Add(sectionType, new Godot.Collections.Dictionary<string, Variant>());
		}

		if(_dataToSave[sectionType].ContainsKey(key)){
			_dataToSave[sectionType][key] = value;
		}
		else{
			_dataToSave[sectionType].Add(key, value);
		}
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

		var dataDictionary = json.Data.AsGodotDictionary<string, Godot.Collections.Dictionary<string, Variant>>();
		SectionType[] sections = Enum.GetValues<SectionType>();

		foreach(KeyValuePair<string, Godot.Collections.Dictionary<string, Variant>> keyValuePair in dataDictionary){
			_dataToSave.Add(sections[int.Parse(keyValuePair.Key)], keyValuePair.Value);
		}
	}

	private bool ExistConfig()
		=> FileAccess.FileExists(GetFullPath());

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
	}

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

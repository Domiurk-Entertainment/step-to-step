using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class DemoSave : Node
{
    private const string SECTION = "Demo";

    [Export] private Sprite2D _node;
    [Export] private Node2D[] points;
    [Export] private float duration = 1;
    [Export] private string configPath = "res://Demo/configs.cfg";

    private int indexPoint;
    private bool isMoving;

    
    public void NextPoint()
    {
        if(indexPoint >= points.Length)
            indexPoint = 0;
        if(isMoving)
            return;
        isMoving = true;
        Tween tween = CreateTween().BindNode(_node);
        indexPoint++;
        tween.TweenProperty(_node, "global_position", points[indexPoint].GlobalPosition, duration);
        tween.Finished += () =>
                              {
                                  isMoving = false;
                                  tween.Kill();
                              };
    }

    public void Save()
    {
        ConfigFile config = new ConfigFile();
        Error errorOpen = config.Load(configPath);

        if(errorOpen == Error.FileNotFound){
            Error errorSave = config.Save(configPath);

            if(errorSave != Error.Ok)
                GD.PrintErr("Save is failed." + errorSave);
            return;
        }

        config.SetValue(SECTION, _node.Name, GetProperty());
        config.SetValue(SECTION, nameof(indexPoint), indexPoint);
        config.Save(configPath);
    }

    public void Load()
    {
        ConfigFile config = new ConfigFile();
        Error errorOpen = config.Load(configPath);

        if(errorOpen != Error.Ok){
            GD.PrintErr("Not data to load");
            return;
        }

        Variant data = config.GetValue(SECTION, _node.Name, GetProperty());
        indexPoint = config.GetValue(SECTION, nameof(indexPoint), indexPoint).AsInt32();
        
        Parse(data.AsGodotDictionary());
    }

    private Dictionary GetProperty()
    {
        var result = new Dictionary();
        result.Add("global_position", _node.GlobalPosition);
        return result;
    }

    private void Parse(Dictionary data)
    {
        foreach(KeyValuePair<Variant, Variant> pair in data){
            _node.Set(pair.Key.AsStringName(), pair.Value);
        }
    }

    private Variant LoadData(string key, Variant defaultData = new Variant())
    {
        ConfigFile config = GetConfigFile();
        Variant result = defaultData;

        foreach(string section in config.GetSections()){
            if(section != "Demo")
                continue;
            result = config.GetValue(section, key, defaultData);
        }

        return result;
    }

    private ConfigFile GetConfigFile()
    {
        var result = new ConfigFile();

        Error error = result.Load(configPath);

        if(error != Error.Ok){
            GD.PrintErr(error);
        }

        return result;
    }
}
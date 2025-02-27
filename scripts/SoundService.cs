using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StepToStep.Sound;

[GlobalClass]
public partial class SoundService : Node
{
    private readonly Dictionary<string,AudioStreamPlayer> sounds = new();

    public override void _Ready()
    {
        foreach(Node child in GetChildren()){
            
            string name = child.Name.ToString().ToLower();
            sounds.Add(name,child as AudioStreamPlayer);
        }
    }

    public void Play(string soundName)
    {

        if(!sounds.TryGetValue(soundName.ToLower(), out AudioStreamPlayer sound)){
            GD.PrintErr($"{Name} is not contains {soundName}");
            return;
        }
        sound.Play();
    }
}
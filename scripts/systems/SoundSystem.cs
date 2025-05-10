using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StepToStep.Systems;

public partial class SoundSystem : System<SoundSystem>
{
	private List<AudioStreamPlayer> _sounds;

	public override void _Ready()
	{
		base._Ready();
		_sounds = new List<AudioStreamPlayer>(GetChildren().Cast<AudioStreamPlayer>());
	}

	public void Add(AudioStreamPlayer sound)
	{
		var duplicate = sound.Duplicate() as AudioStreamPlayer;
		_sounds.Add(duplicate);
		AddChild(duplicate);
		sound.QueueFree();
	}

	public void TryPlay(string name)
	{
		AudioStreamPlayer sound =
			_sounds.FirstOrDefault(a => string.Equals(a.Name, name, StringComparison.CurrentCultureIgnoreCase));
		sound?.Play();
	}
}

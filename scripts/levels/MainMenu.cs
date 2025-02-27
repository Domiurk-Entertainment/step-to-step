using Godot;
using StepToStep.Utils;
using System;

namespace StepToStep.Level;

public partial class MainMenu : Node2D
{
	[Export] private PackedScene _sceneToPlay;
	
	public void StartGame()
	{
		SceneTransition.Instance.ChangeScene(_sceneToPlay);
	}

	public void ExitGame()
	{
		GetTree().Quit();
	}
}

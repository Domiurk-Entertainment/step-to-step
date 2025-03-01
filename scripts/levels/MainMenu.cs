using Godot;
using StepToStep.Utils;

namespace StepToStep.Level;

public partial class MainMenu : Node
{
	[Export(PropertyHint.File)] private string _sceneToPlay;
	public void StartGame()
	{
		SceneTransition.Instance.ChangeScene(GD.Load<PackedScene>(_sceneToPlay));
	}

	public void ExitGame()
	{
		GetTree().Quit();
	}
}

using Godot;
using StepToStep.Utils;

namespace StepToStep.Level;

public partial class MainMenu : Node
{
	[Export(PropertyHint.File)] private string _sceneToPlay;
	public void StartGame()
	{
		PackedScene scene = GD.Load<PackedScene>(_sceneToPlay);
		SceneTransition.Instance.ChangeScene(scene);
	}

	public void ExitGame()
	{
		GetTree().Quit();
	}
}

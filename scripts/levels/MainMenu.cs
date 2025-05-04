using Godot;
using StepToStep.Systems;
using StepToStep.Utils;

namespace StepToStep.Level;

public partial class MainMenu : Node
{
	[Export(PropertyHint.File)] private string _sceneToPlay;
	[Export(PropertyHint.File)] private Button continueButton;

	public override void _Ready()
	{
		// CheckExistData();
	}

	private void CheckExistData()
	{
		continueButton.Disabled = !Utils.Systems.Instance.SaveSystem.ExistSaves();
	}

	public void Start()
	{
		Utils.Systems.Instance.UserInterfaceSystem.Modal.Open("Start New Game",
																	   "Are you sure you want to start the new game?",
																	   "Yes", "No",
																	   StartGame);
		return;

		void StartGame()
		{
			Utils.Systems.Instance.SaveSystem.RemoveAllData();
			Continue();
		}
	}

	public void Exit()
	{
		GetTree().Quit();
	}

	public void Continue()
	{
		PackedScene scene = GD.Load<PackedScene>(_sceneToPlay);
		Utils.Systems.Instance.SceneTransition.ChangeScene(scene);
	}

	public void RemoveSavedData()
	{
		Utils.Systems.Instance.SaveSystem.RemoveAllData();
		CheckExistData();
	}
}

using Godot;
using StepToStep.Systems;
using StepToStep.Utils;

namespace StepToStep.Level;

public partial class MainMenu : Node
{
    [Export(PropertyHint.File)] private string _sceneToPlay;
    [Export()] private Button continueButton;

    public override void _Ready()
    {
        CheckExistData();
    }

    private void CheckExistData()
    {
        continueButton.Disabled = !SaveSystem.Instance.ExistSaves();
    }

    public void Start()
    {
        // StartGame();
        UserInterfaceSystem.Instance.AddModal("Start New Game",
                                              "Are you sure you want to start the new game?",
                                              "Yes", "No",
                                              StartGame);
        return;

        void StartGame()
        {
            UserInterfaceSystem.Instance.PrintTree();

            SaveSystem.Instance.RemoveAllData();
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
        SceneTransition.Instance.ChangeScene(scene);
    }

    public void RemoveSavedData()
    {
        SaveSystem.Instance.RemoveAllData();
        CheckExistData();
    }
}
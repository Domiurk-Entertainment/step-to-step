using Godot;

namespace StepToStep.scripts;

public partial class PausedControl : Control
{
    [Export] private Button pausedButton;
    [Export] private BoxContainer container;
    [Export] private Button resumeButton;
    [Export] private Button exitButton;

    public override void _EnterTree()
    {
        base._EnterTree();
        pausedButton.Pressed += PausedClicked;
        resumeButton.Pressed += ResumeClicked;
        exitButton.Pressed += ExitClicked;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        pausedButton.Pressed -= PausedClicked;
        resumeButton.Pressed -= ResumeClicked;
        exitButton.Pressed -= ExitClicked;
    }

    private void PausedClicked()
    {
        container.Visible = !container.Visible;
    }
    private void ResumeClicked()
    {
        container.Visible = false;
    }

    private void ExitClicked()
    {
        GetTree().Quit();
    }
}
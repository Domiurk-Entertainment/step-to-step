using Godot;
using StepToStep.Systems;

namespace StepToStep.Level;

public partial class CaveMap : Level
{
	public override void _EnterTree()
	{
		UserInterfaceSystem.Instance.ShowPauseButton();
	}

	public override void _ExitTree()
	{
		UserInterfaceSystem.Instance.HidePauseButton();
	}
}

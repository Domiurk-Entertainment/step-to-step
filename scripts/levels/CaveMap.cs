using Godot;
using StepToStep.Systems;

namespace StepToStep.Level;

public partial class CaveMap : Level<LevelConfig>
{
	public override void _EnterTree()
	{
		UserInterfaceSystem.Instance.ShowPauseButton();
		UserInterfaceSystem.Instance.PrintTree();
	}

	public override void _ExitTree()
	{
		UserInterfaceSystem.Instance.HidePauseButton();
	}
}

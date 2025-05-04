using Godot;
using StepToStep.Systems;

namespace StepToStep.Levels;

public partial class CaveMap : Level<LevelConfig>
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

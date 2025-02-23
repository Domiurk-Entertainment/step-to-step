using Godot;
using StepToStep;
using StepToStep.Battle;
using StepToStep.scripts;
using StepToStep.Utilities;
using System;

public partial class DemoBattle : Node
{
	[Export] private Button playerAttackButton;
	[Export] private Player player;
	[Export] private Enemy enemy;
	[Export] private Label debugLabel;

	private bool playerTurn = true;
	private Sight node;

	public override void _Ready()
	{
		playerAttackButton.Pressed += player.Attack;

		enemy.ChangeStep += EnemyOnChangeStep;
		player.ChangeStep += PlayerOnChangeStep;
		node = player.FindChild("sight") as Sight;
	}

	public override void _Process(double delta)
	{
		if(debugLabel == null){
			return;
		}
		debugLabel.Text = $"Rotation({node}):{node.RotationDegrees}";
	}

	private void PlayerOnChangeStep(StepType stepType)
	{
		switch(stepType){
			case StepType.Start:
				break;
			case StepType.End:
				playerAttackButton.Disabled = true;
				SceneTreeTimer timer = GetTree().CreateTimer(2);
				timer.Timeout += () => enemy.TryAttack(player.GlobalPosition);
				break;
			case StepType.Attacked:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(stepType), stepType, null);
		}
	}

	private void EnemyOnChangeStep(StepType stepType)
	{
		switch(stepType){
			case StepType.Start:
				break;
			case StepType.End:
				playerAttackButton.Disabled = false;
				break;
			case StepType.Attacked:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(stepType), stepType, null);
		}
	}
}

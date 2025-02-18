using Godot;
using StepToStep.scripts;
using System;

public partial class DemoBattle : Node
{
	[Export] private Button playerAttackButton;
	[Export] private Player player;
	[Export] private Enemy enemy;

	private bool playerTurn = true;

	public override void _Ready()
	{
		playerAttackButton.Pressed += player.Attack;

		enemy.ChangeStep += EnemyOnChangeStep;
		player.ChangeStep += PlayerOnChangeStep;
	}

	private void PlayerOnChangeStep(StepType stepType)
	{
		GD.Print($"Player is {stepType}");
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
		GD.Print($"Enemy is {stepType}");
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

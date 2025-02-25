using Godot;
using StepToStep;
using StepToStep.Battle;
using StepToStep.InventorySpace;
using StepToStep.scripts;
using StepToStep.Utilities;
using System;

public partial class DemoBattle : Node
{
	[Export] private Button playerAttackButton;
	[Export] private Player player;
	[Export] private Enemy enemy;
	[Export] private Item ball; 

	private Sight node;

	public override void _Ready()
	{
		playerAttackButton.Pressed += player.Attack;
		player.Inventory.AddItem(ball);

		enemy.ChangeStep += EnemyOnChangeStep;
		player.AttackedStep += PlayerOnAttackedStep;
		
		node = player.FindChild("sight") as Sight;
	}
	
	private void PlayerOnAttackedStep(AttackType attackType)
	{
		switch(attackType){
			case AttackType.Start:
				break;
			case AttackType.End:
				playerAttackButton.Disabled = true;
				SceneTreeTimer timer = GetTree().CreateTimer(2);
				timer.Timeout += () => enemy.Attack(player.GlobalPosition);
				break;
			case AttackType.Attacked:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(attackType), attackType, null);
		}
	}

	private async void EnemyOnChangeStep(AttackType attackType)
	{
		switch(attackType){
			case AttackType.Start:
				break;
			case AttackType.End:
				playerAttackButton.Disabled = false;
				// SceneTreeTimer timer = GetTree().CreateTimer(1);
				// await timer.ToSignal(timer,"timeout");
				// enemy.Attack(player.GlobalPosition);
				break;
			case AttackType.Attacked:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(attackType), attackType, null);
		}
	}
}

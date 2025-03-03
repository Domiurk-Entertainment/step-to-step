using Godot;
using StepToStep;
using StepToStep.Battle;
using StepToStep.InventorySpace;
using StepToStep.scripts;
using StepToStep.Utils;
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

		enemy.AttackedStep += EnemyOnAttackedStep;
		player.AttackedStep += PlayerOnAttackedStep;
		
		node = player.FindChild("sight") as Sight;
		
		enemy.ReadyToAttack(player.GlobalPosition);
	}
	
	private void PlayerOnAttackedStep(AttackType attackType)
	{
		switch(attackType){
			case AttackType.Start:
				break;
			case AttackType.End:
				playerAttackButton.Disabled = true;
				SceneTreeTimer timer = GetTree().CreateTimer(2);
				timer.Timeout += enemy.Attack;
				break;
			case AttackType.Attacked:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(attackType), attackType, null);
		}
	}

	private void EnemyOnAttackedStep(AttackType attackType)
	{
		switch(attackType){
			case AttackType.Start:
				break;
			case AttackType.End:
				playerAttackButton.Disabled = false;
				break;
			case AttackType.Attacked:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(attackType), attackType, null);
		}
	}
}

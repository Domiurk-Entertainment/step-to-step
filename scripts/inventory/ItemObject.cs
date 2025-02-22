using Godot;
using System;

namespace StepToStep.Inventory
{
	public partial class ItemObject : Node2D
	{
		[Export] private ItemResource item;
		[Export] private int amount;

		public override void _Ready()
		{
			GD.Print($"{item.ID}:{amount}");
		}
	}
}
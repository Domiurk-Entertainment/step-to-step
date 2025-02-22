using Godot;
using System;

namespace StepToStep.Inventory
{
	public partial class ItemInInventory : Button
	{
		[Export] private string format = "({0})";
		public Item _item;
		public void SetItem(Item item)
		{
			_item = item;
			Icon = item.Resource.Icon;
			UpdateItem();
		}

		public void UpdateItem()
		{
			Text = string.Format(format,_item.Amount);
		}
	}
}
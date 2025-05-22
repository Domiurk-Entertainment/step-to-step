using Godot;
using System;

namespace StepToStep.InventorySystem;

public partial class InventoryInterface : Control
{
	private const string NODE_NAME = "Inventory";

	[Export] private Container _container;
	[Export] private string _formatBall = "{0}";

	[Export] private Vector2 _slotSizeConst = new Vector2(50, 31);
	private IInventory _inventory;

	public void Initialize(IInventory inventory)
	{
		_inventory = inventory;

		if(_inventory == null){
			throw new NullReferenceException("Inventory Interface is null");
		}

		_inventory.AddedItem += InventoryOnAddedItem;
		_inventory.RemovedItem += InventoryOnRemovedItem;
		_inventory.TakenItem += InventoryOnTakenItem;
	}

	public override void _ExitTree()
	{
		_inventory.AddedItem -= InventoryOnAddedItem;
		_inventory.RemovedItem -= InventoryOnRemovedItem;
		_inventory.TakenItem -= InventoryOnTakenItem;
	}

	private void InventoryOnTakenItem(Item item)
	{
		if(!string.IsNullOrEmpty(_formatBall))
			((Button)_container.GetChildren()[0]).Text = string.Format(_formatBall, item.Amount);
	}

	private void InventoryOnRemovedItem(Item item)
	{
		_container.GetChildren()[0].QueueFree();
	}

	private void InventoryOnAddedItem(Item item)
	{
		Button slot = CreateItemSlot();
		if(!string.IsNullOrEmpty(_formatBall))
			slot.Text = string.Format(_formatBall, item.Amount);
		slot.Icon = item.Resource.Icon;
		_container.AddChild(slot);
	}

	private Button CreateItemSlot()
	{
		Button result = new Button();

		if(!string.IsNullOrEmpty(_formatBall)){
			result.ExpandIcon = true;
		}
		else{
			result.IconAlignment = HorizontalAlignment.Center;
		}
		result.FocusMode = FocusModeEnum.None;
		result.ButtonMask = 0;
		result.Alignment = HorizontalAlignment.Left;
		result.MouseFilter = MouseFilterEnum.Ignore;
		result.Flat = true;
		result.Size = _slotSizeConst;
		return result;
	}
}

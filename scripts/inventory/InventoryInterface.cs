using Godot;
using System;

namespace StepToStep.InventorySpace;

public partial class InventoryInterface : Control
{
    private const string NODE_NAME = "Inventory";

    [Export] private VBoxContainer _vBoxContainer;
    [Export] private string _formatBall = "{0}";

    private readonly Vector2 _slotSizeConst = new Vector2(50, 31);
    private IInventory _inventory;

    public override void _EnterTree()
    {
        _inventory = GetTree().CurrentScene.FindChild(NODE_NAME) as IInventory;

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
        ((Button)_vBoxContainer.GetChildren()[0]).Text = string.Format(_formatBall, item.Amount);
    }

    private void InventoryOnRemovedItem(Item item)
    {
        _vBoxContainer.GetChildren()[0].QueueFree();
    }

    private void InventoryOnAddedItem(Item item)
    {
        Button slot = CreateItemSlot();
        slot.Text = string.Format(_formatBall, item.Amount);
        slot.Icon = item.Resource.Icon;
        _vBoxContainer.AddChild(slot);
    }


    private Button CreateItemSlot()
    {
        Button result = new Button();
        result.FocusMode = FocusModeEnum.None;
        result.ButtonMask = 0;
        result.Alignment = HorizontalAlignment.Left;
        result.ExpandIcon = true;
        result.MouseFilter = MouseFilterEnum.Ignore;
        result.Flat = true;
        result.Size = _slotSizeConst;
        return result;
    }
}
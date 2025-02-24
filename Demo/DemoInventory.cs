using Godot;
using StepToStep;
using StepToStep.InventorySpace;

public partial class DemoInventory : Node2D
{
    [Export] private Player _player;
    [Export] private Item[] _inventoryItems;

    private IInventory _inventory;

    public override void _Ready()
    {
        _inventory = _player.Inventory;
        _inventory.AddItems(_inventoryItems);
    }
}
using Godot;
using StepToStep.ShopSystem;

namespace StepToStep.InventorySpace;

[GlobalClass]
public partial class ItemResource : Resource
{
    public string ID => _id;
    public string Description => _description;
    public Texture2D Icon => _icon;
    [field: Export] public int Cost { get; private set; } = 0;

    [Export] private string _id;
    [Export] private string _description;
    [Export] private Texture2D _icon;

    public ItemResource()
    {
        _id = "";
        _description = "";
        _icon = null;
    }

}
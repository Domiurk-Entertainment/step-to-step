using Godot;

namespace StepToStep.Inventory;

[GlobalClass]
public partial class ItemResource : Resource
{
    public string ID => Id;
    public string Description => description;
    public int MaxAmount => maxAmount;
    public Texture Icon => icon;

    [Export] public string Id;
    [Export] public string description;
    [Export] public int maxAmount;
    [Export] public Texture icon;
}
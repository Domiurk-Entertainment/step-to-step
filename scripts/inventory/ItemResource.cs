using Godot;

namespace StepToStep.Inventory;

[GlobalClass]
public partial class ItemResource : Resource
{
    public string ID => Id;
    public string Description => description;
    public Texture Icon => icon;

    [Export] public string Id;
    [Export] public string description;
    [Export] public Texture icon;
}
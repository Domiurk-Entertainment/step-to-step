using Godot;

namespace StepToStep.InventorySpace;

[GlobalClass]
public partial class Item : Resource
{
    [Export] public BallResource Resource;
    [Export] public int Amount;
    public override string ToString() => $"{Resource.ID}:{Amount}";

    public Item(BallResource resource, int amount)
    {
        Resource = resource;
        Amount = amount;
    }

    public Item()
    {
        Resource = null;
        Amount = 0;
    }
}
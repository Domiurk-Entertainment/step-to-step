using Godot;

namespace StepToStep.InventorySpace;

[GlobalClass]
public partial class Item : Resource
{
    [field: Export] public BallResource Resource { get; private set; }
    [field: Export] public int Amount;

    public override string ToString() => $"{Resource.ID}:{Amount}";

    public Item(BallResource resource, int amount, int price)
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
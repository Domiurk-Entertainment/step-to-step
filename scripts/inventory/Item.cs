using Godot;

namespace StepToStep.InventorySystem;

[GlobalClass]
public partial class Item : Resource, IItem
{
    [Export] public BallResource Resource { get; private set; }
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

public interface IItem
{
    BallResource Resource { get; }
}
using Godot;

namespace StepToStep.Inventory;

[GlobalClass]
public partial class Item : Resource
{
    [Export] public BallResource Resource;
    [Export] public int Amount;
    public override string ToString() => $"{Resource.ID}:{Amount}";
    public bool IsEmpty() => Resource == null;

    public Item GetAndClear()
    {
        Item item = GetCopy();
        Resource = null;
        Amount = 0;
        return item;
    }

    public Item GetCopy()
    {
        return new Item(Resource, Amount);
    }

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
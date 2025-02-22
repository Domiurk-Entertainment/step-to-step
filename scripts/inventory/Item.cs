namespace StepToStep.Inventory;

public class Item
{
    public ItemResource Resource;
    public int Amount;
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

    public Item(ItemResource resource, int amount)
    {
        Resource = resource;
        Amount = amount;
    }
}
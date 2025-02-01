namespace StepToStep.Inventory;

public class Item
{
    public ItemResource Resource;
    public float Amount;
    public override string ToString() => $"{Resource.ID}:{Amount}";
}
namespace StepToStep.InventorySpace;

public interface IInventory
{
    event InventoryDelegate AddedItem;
    event InventoryDelegate RemovedItem;
    event InventoryDelegate TakenItem;

    public void AddItems(Item[] items);
    public void AddItem(Item item);
    public Item GetBall();
}
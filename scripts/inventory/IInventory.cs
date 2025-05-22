using System.Collections.Generic;

namespace StepToStep.InventorySystem;

public interface IInventory
{
    event InventoryDelegate AddedItem;
    event InventoryDelegate RemovedItem;
    event InventoryDelegate TakenItem;

    IReadOnlyCollection<Item> Balls { get; }
    IItem CurrentBall { get; }
    
    public void AddItems(Item[] items);
    public void AddItem(Item item);
    public Item GetBall();
}
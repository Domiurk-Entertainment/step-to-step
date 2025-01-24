using System;

namespace StepToStep.Inventory;

public class Slot
{
    public event Action<Item> AddedItem; 
    public event Action Cleared; 
    public Item item;

    public void SetItem(Item item)
    {
        this.item = item;
        AddedItem?.Invoke(item);
    }

    public void Clear()
    {
        item = null;
        Cleared?.Invoke();
    }

    public override string ToString() => item is null ? "Empty" : item.ToString();
}
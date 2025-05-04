using Godot;
using Godot.Collections;
using StepToStep.InventorySpace;
using StepToStep.ShopSystem;

namespace StepToStep.Levels;

public partial class Shop : Node
{
    [Export] private Array<Item> _items;
    [Export] private ItemList _visualItem;
    public bool TryBuy(IBuying item)
    {/*
        int result = Coins - item.BuyCost;

        if(result < 0)
            return false;
        
        Coins -= item.BuyCost;
        OnCoinValueChanged?.Invoke(Coins);*/
        return true;
    }
}
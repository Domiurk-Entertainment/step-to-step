using Godot;
using System;

namespace StepToStep.Inventory;

public partial class ItemContainer : Node
{
    [Export] private int capacity = 3;
    
    private Slot[] slots;

    public override void _Ready()
    {
        slots = new Slot[capacity];
    }

    private void FillSlots()
    {
        for(int i = 0; i < slots.Length; i++){
            slots[i] = new Slot();
        }
    }
}
using Godot;
using StepToStep.ShopSystem;
using StepToStep.Systems;
using System;

namespace StepToStep.Utils;

public partial class Currency : Label
{
    [field: Export] public int Coins { get; private set; } = 0;

    [Export] private SectionType _sectionType = SectionType.Player;

    private string GetKey(string additional)
    {
        string result = $"{GetParent().Name}/{Name}";
        if(string.IsNullOrEmpty(additional))
            result = $"{result}/{additional}";
        return result;
    }
    
    public override void _EnterTree()
    {
        // Coins = SaveSystem.Instance.Get(_sectionType, GetKey("Coins"), 0).AsInt32();
    }

    public override void _ExitTree()
    {
        SaveSystem.Instance.Set(_sectionType, GetKey("Coins"), Coins);
    }
}
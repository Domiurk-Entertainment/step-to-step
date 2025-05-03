using Godot;
using StepToStep.Systems;

namespace StepToStep.Utils;

public partial class Systems : Node
{
    public static Systems Instance;

     public SaveSystem SaveSystem;
    public SceneTransition SceneTransition;
    public UserInterfaceSystem UserInterfaceSystem;
    
    public override void _Ready()
    {
        if(SaveSystem.ExistSaves())
            SaveSystem.LoadData();
    }
}
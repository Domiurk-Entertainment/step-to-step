using Godot;
using StepToStep.Interface;

namespace StepToStep.Systems
{
    public partial class UserInterfaceSystem : Control
    {
        public static UserInterfaceSystem Instance;

        [Export] public Modal Modal;

        public override void _Ready()
        {
            if(Instance != null && Instance != this){
                QueueFree();
                GD.PrintErr("Instance already exists");
                return;
            }

            Instance = this;
        }
    }
}
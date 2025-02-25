using Godot;
using StepToStep.Utils;

namespace StepToStep.Level;

public partial class Level : Node2D
{
    public void BackToLastScene()
    {
        SceneTransition.Instance.ChangeScene(SceneTransition.ScenesHistory.Pop());
    }

    public void SaveCurrentScene()
    {
        
    }
}
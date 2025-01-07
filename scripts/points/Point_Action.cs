using Godot;
using System;

namespace StepToStep.Points;

public partial class Point_Action : Resource
{
    [Export] private NodePath sceneToLoad;
    // видалити разом з Point Manager
    /// <summary>
    /// Void for selected point
    /// </summary>
    /// <param name="nodeToInstance">Node Where Instance To Needed Scene</param>
    public void Select(Node2D nodeToInstance)
    {
        nodeToInstance.AddChild(GD.Load<PackedScene>(sceneToLoad).Instantiate());
    }
}
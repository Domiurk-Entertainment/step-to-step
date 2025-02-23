using Godot;
using System;
using System.Collections.Generic;

namespace StepToStep.scripts;

public partial class Point : Button
{
    public IReadOnlyCollection<Point> Points => pointsUnlock;

    [Export] private Point[] pointsUnlock = Array.Empty<Point>();
    [Export(PropertyHint.Dir)] private PackedScene sceneToLoad;

    public void ActivePoint()
    {
        if(sceneToLoad != null){
            GetNode("/root/SceneTransition").Call("change_scene", sceneToLoad.ResourcePath);
        }

        foreach(Point point in pointsUnlock)
            point.ChangePointVisible(true);
    }

    public void ChangePointVisible(bool visible)
    {
        Disabled = !visible;
    }
}
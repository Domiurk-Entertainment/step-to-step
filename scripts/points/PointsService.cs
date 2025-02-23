using Godot;
using StepToStep.Utils;
using System.Linq;

namespace StepToStep.Level;

public partial class PointsService : Node2D
{
    private const string GROUP = "Points";

    public PointsService Instance;

    [Export] private Node2D miniPlayer;
    [Export] private Point startPoint;
    [Export] private float duration = 1;
    [Export] private Vector2 pointOffset = new Vector2(50, 50);

    private Point lastPoint;
    private Node _sceneTransition;

    public override void _Ready()
    {
        if(Instance == null)
            Instance = this;
        else{
            GD.PrintErr($"{this} is dublicated in {GetTree().CurrentScene.Name} scene");
            QueueFree();
        }

        _sceneTransition = GetNode("/root/SceneTransition");

        Point[] points = GetTree().GetNodesInGroup(GROUP).Cast<Point>().ToArray();

        foreach(Point point in points){
            point.Pressed += () => PointOnPressed(point);
            point.ChangePointVisible(false);
        }

        startPoint ??= points[0];
        lastPoint = startPoint;
        miniPlayer.GlobalPosition = startPoint.GlobalPosition + pointOffset;
        startPoint.ChangePointVisible(false);
        ActivateClickedPoint(startPoint);
    }

    private void PointOnPressed(Point point)
    {

        foreach(Point lastPointChild in lastPoint.Points)
            lastPointChild.ChangePointVisible(false);

        Tween tween =
            miniPlayer.CreateToTween(miniPlayer.GlobalPosition, point.GlobalPosition + pointOffset,
                                     "global_position",
                                     duration);

        tween.Finished += TweenOnFinished;
        return;

        void TweenOnFinished()
        {
            lastPoint = point;
            ActivateClickedPoint(point);
            tween.Kill();
        }
    }

    private void ActivateClickedPoint(Point point)
    {
        if(point.SceneToLoad != null)
            SceneTransition.Data.Add(point.SceneToLoad.ResourcePath, point.Config);
        
        if(point.SceneToLoad != null){
            _sceneTransition.Call("ChangeScene", point.SceneToLoad);
        }

        foreach(Point childPoint in point.Points)
            childPoint.ChangePointVisible(true);
    }
}
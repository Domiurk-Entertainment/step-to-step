using Godot;
using System.Linq;

namespace StepToStep.scripts;

public partial class PointsService : Node2D
{
    public PointsService Instance;

    private Point[] points;
    [Export] private Node2D miniPlayer;
    [Export] private Point startPoint;
    [Export] private float duration = 1;
    [Export] private Vector2 pointOffset = new Vector2(50, 50);

    private Point lastPoint;

    public override void _Ready()
    {
        if(Instance == null)
            Instance = this;
        else{
            GD.PrintErr($"{this} is dublicated in {GetTree().CurrentScene.Name} scene");
            QueueFree();
        }

        points = GetChildren().Cast<Point>().ToArray();

        foreach(Point point in points){
            point.Pressed += PointOnPressed;
            point.ChangePointVisible(false);
            continue;

            void PointOnPressed()
            {
                foreach(Point lastPointChild in lastPoint.Points)
                    lastPointChild.ChangePointVisible(false);
                GD.Print(point.PivotOffset);
                Tween tween =
                    miniPlayer.CreateToTween(miniPlayer.GlobalPosition, point.GlobalPosition + pointOffset,
                                             "global_position",
                                             duration);
                tween.Finished += TweenOnFinished;
                return;

                void TweenOnFinished()
                {
                    point.ActivePoint();
                    lastPoint = point;
                    tween.Kill();
                }
            }
        }

        startPoint ??= points[0];
        lastPoint = startPoint;
        miniPlayer.GlobalPosition = startPoint.GlobalPosition + pointOffset;
        startPoint.ChangePointVisible(false);
        startPoint.ActivePoint();
    }
}
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
                Tween tween =
                    miniPlayer.CreateToTween(miniPlayer.GlobalPosition, point.GlobalPosition, "global_position",
                                             duration);
                tween.Finished += TweenOnFinished;
                return;

                void TweenOnFinished()
                {
                    point.ActivePoint();
                    tween.Kill();
                }
            }
        }

        startPoint ??= points[0];

        miniPlayer.GlobalPosition = startPoint.GlobalPosition;
        startPoint.ChangePointVisible(true);
        startPoint.ActivePoint();
    }
}
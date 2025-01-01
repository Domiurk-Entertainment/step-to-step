using Godot;
using System.Linq;

namespace StepToStep;

public partial class Points : Node2D
{
    [Export]
    private Node2D Player;

    private Point[] points;

    public override void _EnterTree()
    {
        points = GetChildren().Where(child => child is Point).Cast<Point>().ToArray();

        foreach(Point point in points){
            point.ClickedToPoint += OnClickedToPoint;
        }

        return;
    }

    private void OnClickedToPoint(Vector2 position)
    {
        Player.GlobalPosition = position;
    }

    public override void _ExitTree()
    {
        foreach(Point point in points){
            point.ClickedToPoint -= OnClickedToPoint;
        }
    }
}
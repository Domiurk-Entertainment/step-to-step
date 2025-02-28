using Godot;
using StepToStep.Systems;
using StepToStep.Utils;
using System.Collections.Generic;
using System.Linq;

namespace StepToStep.Level;

public partial class PointsService : Node
{
    private const string GROUP = "Points";

    [Export] private Node2D _miniPlayer;
    [Export] private Vector2 _pointOffset = new(50, 50);
    [ExportCategory("Tween Setting")]
    [Export] private float _duration = 1;
    [Export] private Tween.TransitionType _transitionType = Tween.TransitionType.Linear;

    private TypeConfiguration _saveConfigurationType = TypeConfiguration.Level;
    private readonly List<Point> _points = new();
    private Point _currentPoint;
    private Point _lastPoint;

    private string GetKey() => $"{GetTree().CurrentScene.SceneFilePath.GetBaseName()}/{Name}";

    public override void _Ready()
    {
        _points.Clear();
        _points.AddRange(GetChildren().Cast<Point>());

        foreach(Point point in _points){
            point.Pressed += () => PointOnPressed(point);
            point.ChangePointVisible(false);
        }

        int index = SaveSystem.Instance.LoadIntData(_saveConfigurationType, GetKey(), 0);

        _currentPoint = _points[index];
        _lastPoint = _currentPoint;
        _miniPlayer.GlobalPosition = _currentPoint.GlobalPosition + _pointOffset;
        _currentPoint.ChangePointVisible(false);
        ActivateClickedPoint(_currentPoint);
    }

    private void PointOnPressed(Point point)
    {
        _currentPoint = point;

        foreach(Point lastPointChild in _lastPoint.Points)
            lastPointChild.ChangePointVisible(false);

        Tween tween =
            _miniPlayer.CreateToTween(_miniPlayer.GlobalPosition, point.GlobalPosition + _pointOffset,
                                      "global_position",
                                      _duration, _transitionType);

        tween.Finished += TweenOnFinished;
        return;

        void TweenOnFinished()
        {
            _lastPoint = point;
            ActivateClickedPoint(point);
            tween.Kill();
        }
    }

    private void ActivateClickedPoint(Point point)
    {
        if(point.SceneToLoad != null)
            SceneTransition.Data.Add(point.SceneToLoad.ResourcePath, point.Config);

        if(point.SceneToLoad != null){
            SceneTransition.Instance.ChangeScene(point.SceneToLoad);
            SaveSystem.Instance.SaveData(_saveConfigurationType, GetKey(), _points.IndexOf(_currentPoint));
        }

        foreach(Point childPoint in point.Points)
            childPoint.ChangePointVisible(true);
    }
}
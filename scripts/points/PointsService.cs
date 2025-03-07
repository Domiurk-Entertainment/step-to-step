using Godot;
using StepToStep.Systems;
using StepToStep.Utils;
using System.Collections.Generic;
using System.Linq;

namespace StepToStep.Level;

public partial class PointsService : Node
{
    private const string PLAYER_IDLE_ANIMATION_NAME = "idle";
    private const string PLAYER_RUN_ANIMATION_NAME = "run";

    [Export] private Node2D _miniPlayer;
    [Export] private Vector2 _pointOffset = new(50, 50);
    [ExportCategory("Tween Setting")]
    [Export] private float _duration = 1;
    [Export] private Tween.TransitionType _transitionType = Tween.TransitionType.Linear;

    private static TypeConfiguration _saveConfigurationType = TypeConfiguration.Level;
    private readonly List<Point> _points = new();
    private Point _currentPoint;
    private Point _lastPoint;
    private AnimatedSprite2D _animation;
    private AudioStreamPlayer _walkSound;

    private string GetKey(string additionalKey = "")
    {
        const char separator = '/';

        if(string.IsNullOrEmpty(additionalKey))
            return $"{GetTree().CurrentScene.SceneFilePath.GetBaseName()}/{Name}";

        if(additionalKey.StartsWith(separator.ToString()))
            return $"{GetTree().CurrentScene.SceneFilePath.GetBaseName()}/{Name}" + additionalKey;
        return $"{GetTree().CurrentScene.SceneFilePath.GetBaseName()}/{Name}" + separator + additionalKey;
    }

    public override void _Ready()
    {
        _animation = _miniPlayer.GetNode<AnimatedSprite2D>("Animations");
        _walkSound = GetNode<AudioStreamPlayer>("%Walk");

        foreach(Point point in _points){
            point.Pressed += () => PointOnPressed(point);
            point.Disabled = true;
        }

        _lastPoint = _currentPoint;
        _miniPlayer.GlobalPosition = _currentPoint.GlobalPosition + _pointOffset;
        _currentPoint.Disabled = true;
        ActivateClickedPoint(_currentPoint);
    }

    public override void _EnterTree()
    {
        _points.Clear();
        _points.AddRange(GetChildren().Cast<Point>());

        int index = SaveSystem.Instance.LoadIntData(_saveConfigurationType, GetKey(), 0);
        _currentPoint = _points[index];
        _currentPoint.Visited = SaveSystem.Instance.LoadIntData(_saveConfigurationType, GetKey(_currentPoint.Name), 0);
    }

    public override void _ExitTree()
    {
        SaveSystem.Instance.SaveData(_saveConfigurationType, GetKey(_currentPoint.Name), _currentPoint.Visited);
        SaveSystem.Instance.SaveData(_saveConfigurationType, GetKey(), _points.IndexOf(_currentPoint));
    }

    private void PointOnPressed(Point point)
    {
        _animation.Play(PLAYER_RUN_ANIMATION_NAME);
        _animation.FlipH = (point.GlobalPosition - _currentPoint.GlobalPosition).Normalized().X < 0;
        _walkSound.Play();
        _currentPoint = point;

        foreach(Point lastPointChild in _lastPoint.Points)
            lastPointChild.Disabled = true;

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
            _animation.Play(PLAYER_IDLE_ANIMATION_NAME);
            _walkSound.Stop();
            tween.Kill();
        }
    }

    private void ActivateClickedPoint(Point point)
    {
        if(point.CanVisit()){
            point.Visited++;

            if(point.Config != null)
                SceneTransition.Data.Add(point.SceneToLoad.ResourcePath, point.Config);

            if(point.SceneToLoad != null){
                SceneTransition.Instance.ChangeScene(point.SceneToLoad);
            }
        }

        foreach(Point childPoint in point.Points)
            childPoint.Disabled = false;
    }
}
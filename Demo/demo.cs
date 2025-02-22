using Godot;
using System;

public partial class demo : Node2D
{
    [Export] private Vector3[] positions;
    [Export] private Node2D experimentObject;
    private int currentPosition;
    private Tween _tween;

    public override void _Ready()
    {
        _tween = CreateTween();
        _tween.Pause();
    }

    public override void _Input(InputEvent @event)
    {
        if(Input.IsKeyPressed(Key.Space)){
            if(currentPosition == positions.Length)
                currentPosition = 0;
            _tween.TweenProperty(experimentObject, "global_position", positions[currentPosition], 1);
            _tween.Play();
        }
    }
}
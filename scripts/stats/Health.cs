using Godot;
using System;

namespace StepToStep;

[GlobalClass]
public partial class Health : ProgressBar
{
    public event Action<float> ChangedValue;

    [ExportCategory("Float Number")]
    [Export] private float _duration = 1;
    [Export] private float _length = 50;
    [Export] private Font _font;
    [Export] private Tween.TransitionType _transitionType = Tween.TransitionType.Elastic;

    public void Subtract(Node sender, float value)
    {
        if(value < 0)
            throw new Exception($"{sender} sending failure value ({value})");
        Value -= (value);
        ChangedValue?.Invoke((float)Value);
        CreateFloatNumbers(value);
    }

    public void Add(object sender, float value)
    {
        if(value < 0)
            throw new Exception($"{sender} sending failure value ({value})");
        Value += value;
        ChangedValue?.Invoke((float)Value);
        CreateFloatNumbers(value, "+#.##");
    }

    private void CreateFloatNumbers(float value, string format = "-#.##")
    {
        var label = new Label();
        Vector2 finalVal = Vector2.Up * _length;
        label.LabelSettings = new LabelSettings();
        label.LabelSettings.FontSize = 12;
        label.LabelSettings.Font = _font;
        label.SelfModulate = new Color(1f, 1f, 1f);
        label.Text = value.ToString(format);
        label.Position = new Vector2((float)GD.RandRange(0, Size.X), (float)GD.RandRange(0, Size.Y));
        AddChild(label);

        Tween tween = CreateTween().SetParallel().SetTrans(_transitionType);
        tween.TweenProperty(label, "position", label.Position + finalVal, _duration);
        tween.TweenProperty(label, "self_modulate", new Color(1, 1, 1, 0), _duration);
        tween.Finished += label.QueueFree;
    }
}
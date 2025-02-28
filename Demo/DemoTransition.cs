using Godot;
using System;

public partial class DemoTransition : Control
{
    [Export] private Control _element;
    [Export] private Container _container;
    [Export] private float _duration = 1f;
    // 20 -> 450

    public override void _Ready()
    {
        foreach(Tween.TransitionType transitionType in Enum.GetValues<Tween.TransitionType>()){
            _container.AddChild(new HSeparator());
            var node = _element.Duplicate() as VSplitContainer;
            var boy = node.GetNode("slider") as HSlider;
            var name = node.GetNode("name") as Label;
            Tween tween = CreateTween().BindNode(boy);
            name.Text = $"Transition type:{transitionType.ToString()}";
            node.Show();
            tween.TweenProperty(boy, "value", 100, _duration).SetTrans(transitionType);
            tween.Finished += Finished;
            _container.AddChild(node);
            continue;

            void Finished()
            {
                Tween secondTween = CreateTween().BindNode(boy);
                secondTween.TweenProperty(boy, "value", boy.Value <=1 ? 100 : 0, _duration).SetTrans(transitionType);
                secondTween.Finished += Finished;
            }
        }
        _container.AddChild(new HSeparator());
    }
}
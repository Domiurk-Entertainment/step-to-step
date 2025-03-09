using Godot;
using StepToStep.InventorySpace;
using StepToStep.Utils;

namespace StepToStep.Entity;

public partial class Chest : Node
{
    private const string ANIMATION = "open";

    [Export] private Item _itemReward;
    [Export] private AnimationPlayer _animation;
    [Export] private Sprite2D _itemSprite;

    public override void _Ready()
    {
        GetNode<Button>("Button").Pressed += () => { };

        _animation.AnimationFinished += OnAnimationFinished;

        return;

        void OnAnimationFinished(StringName animName) => _animation.PlaybackActive = false;
    }

    private void BackToMap()
    {
        SceneTransition.Instance.LoadLastScene();
    }

    public void Open()
    {
        if(_animation.CurrentAnimation == ANIMATION){
            SceneTransition.Instance.LoadLastScene();
            return;
        }

        _itemSprite.Texture = _itemReward.Resource.Icon;
        _animation.Play(ANIMATION);
    }
}
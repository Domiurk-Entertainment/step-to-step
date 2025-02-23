using Godot;
using Godot.Collections;
using System;

namespace StepToStep.Utils
{
    public partial class SceneTransition : Node
    {
        public static Dictionary<string, Variant> Data = new();
        
        private const string ANIMATION_NAME_ENTER = "enter";
        private const string ANIMATION_NAME_EXIT = "exit";

        [Export] private AnimationPlayer _animationPlayer;

        private PackedScene lastPackedScene;

        public override void _Ready()
        {
            _animationPlayer.AnimationFinished += OnAnimationFinished;
        }

        private void OnAnimationFinished(StringName animName)
        {
            if(animName == ANIMATION_NAME_EXIT)
                return;
            GetTree().ChangeSceneToPacked(lastPackedScene);
            _animationPlayer.Play(ANIMATION_NAME_EXIT);
        }

        public void ChangeScene(PackedScene target)
        {
            lastPackedScene = target;
            _animationPlayer.Play(ANIMATION_NAME_ENTER);
        }
    }
}
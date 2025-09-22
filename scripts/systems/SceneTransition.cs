using Godot;
using StepToStep.Utils;
using System.Collections.Generic;

namespace StepToStep.Systems
{
    public partial class SceneTransition : System<SceneTransition>
    {
        public static readonly Stack<PackedScene> ScenesHistory = new();

        private const string ANIMATION_NAME_ENTER = "enter";
        private const string ANIMATION_NAME_EXIT = "exit";

        [Export] private AnimationPlayer _animationPlayer;

        private PackedScene _currentPackedScene;

        public override void _Ready()
        {
            base._Ready();
            _animationPlayer.AnimationFinished += OnAnimationFinished;
            _currentPackedScene = GD.Load<PackedScene>(GetTree().CurrentScene.GetSceneFilePath());
        }

        public override void _ExitTree()
        {
            _animationPlayer.AnimationFinished -= OnAnimationFinished;
        }

        private void OnAnimationFinished(StringName animName)
        {
            if(animName == ANIMATION_NAME_EXIT)
                return;
            GetTree().ChangeSceneToPacked(_currentPackedScene);
            _animationPlayer.Play(ANIMATION_NAME_EXIT);
        }

        public void ChangeScene(PackedScene target)
        {
            ScenesHistory.Push(_currentPackedScene);
            _currentPackedScene = target;
            _animationPlayer.Play(ANIMATION_NAME_ENTER);
        }

        public void LoadLastScene()
        {
            if(_animationPlayer.IsPlaying())
                return;
            _currentPackedScene = ScenesHistory.Pop();
            _animationPlayer.Play(ANIMATION_NAME_ENTER);
        }
    }
}
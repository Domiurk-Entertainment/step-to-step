using Godot;
using System.Collections.Generic;

namespace StepToStep.Utils
{
    public partial class SceneTransition : Node
    {
        public static SceneTransition Instance;
        public static readonly Stack<PackedScene> ScenesHistory = new();
        public static readonly Dictionary<string, Variant> Data = new();

        private const string ANIMATION_NAME_ENTER = "enter";
        private const string ANIMATION_NAME_EXIT = "exit";

        [Export] private AnimationPlayer _animationPlayer;

        private PackedScene currentPackedScene;

        public override void _Ready()
        {
            if(Instance != null && Instance != this)
                QueueFree();
            else
                Instance = this;

            _animationPlayer.AnimationFinished += OnAnimationFinished;
            currentPackedScene =
                GD.Load<PackedScene>(ProjectSettings.GetSetting("application/run/main_scene").AsString());
        }

        public override void _ExitTree()
        {
            _animationPlayer.AnimationFinished -= OnAnimationFinished;
        }

        private void OnAnimationFinished(StringName animName)
        {
            if(animName == ANIMATION_NAME_EXIT)
                return;
            GetTree().ChangeSceneToPacked(currentPackedScene);
            _animationPlayer.Play(ANIMATION_NAME_EXIT);
        }

        public void ChangeScene(PackedScene target)
        {
            ScenesHistory.Push(currentPackedScene);
            currentPackedScene = target;
            _animationPlayer.Play(ANIMATION_NAME_ENTER);
        }

        public static Variant GetData(string key) => !Data.Remove(key, out Variant result) ? default : result;

        public void LoadLastScene()
        {
            ChangeScene(ScenesHistory.Pop());
        }
    }
}
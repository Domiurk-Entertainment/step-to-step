using Godot;
using StepToStep.Interface;
using StepToStep.Utils;
using System;
using System.Collections.Generic;

namespace StepToStep.Systems
{
    public partial class UserInterfaceSystem : System<UserInterfaceSystem>
    {
        private readonly Queue<CanvasItem> _queue = new Queue<CanvasItem>();

        [Export] private Modal _modal;
        [Export] public Currency Currency;
        [Export] private Panel _pausePanel;
        [Export] private Button _pauseButton;

        public override void _Ready()
        {
            base._Ready();
            _pauseButton.Pressed += PauseButtonOnPressed;
            var resumeButton = (Button)FindChild("Resume");
            var quitButton = (Button)FindChild("Quit");

            resumeButton.Pressed += ResumeButtonOnPressed;
            quitButton.Pressed += QuitButtonOnPressed;

            return;

            void PauseButtonOnPressed()
            {
                _pausePanel.Show();
                _pauseButton.Hide();
            }
        }

        public void AddModal(string title = "",
                             string content = "",
                             string textOneAction = "OK",
                             string textTwoAction = "Cancel",
                             Action oneAction = null,
                             Action twoAction = null)
        {
            var duplicate = _modal.Duplicate() as Modal;
            duplicate!.Initial(title, content, textOneAction, textTwoAction,
                               oneAction == null ? null : OneActionOnPressed,
                               twoAction == null ? null : TwoActionOnPressed);
            duplicate.Hidden += DuplicateOnHidden;

            bool open = _queue.Count == 0;
            AddChild(duplicate);

            _queue.Enqueue(duplicate);

            if(open)
                _queue.Dequeue().Show();
            return;

            void OneActionOnPressed()
            {
                oneAction?.Invoke();
                duplicate.Close();
            }

            void TwoActionOnPressed()
            {
                twoAction?.Invoke();
                duplicate.Close();
            }

            void DuplicateOnHidden()
            {
                if(_queue.Count > 0)
                    _queue.Dequeue().Show();
                duplicate.QueueFree();
            }
        }

        private void QuitButtonOnPressed()
        {
            ResumeButtonOnPressed();
            SceneTransition.Instance.LoadLastScene();
        }

        private void ResumeButtonOnPressed()
        {
            _pauseButton.Show();
            _pausePanel.Hide();
        }

        public void ShowPauseButton()
        {
            _pauseButton.Show();
        }

        public void HidePauseButton()
        {
            _pauseButton.Hide();
        }

        public void ShowUserInterfaces(UserInterfacesType interfaces)
        {
            HideUserInterfaces(UserInterfacesType.CoinSystem & UserInterfacesType.PauseButton &
                               UserInterfacesType.PausePanel);
            if(interfaces.HasFlag(UserInterfacesType.None))
                return;

            if(interfaces.HasFlag(UserInterfacesType.CoinSystem) || interfaces.HasFlag(UserInterfacesType.All))
                Currency.Show();
            if(interfaces.HasFlag(UserInterfacesType.PauseButton) || interfaces.HasFlag(UserInterfacesType.All))
                _pauseButton.Show();
            if(interfaces.HasFlag(UserInterfacesType.PausePanel) || interfaces.HasFlag(UserInterfacesType.All))
                _pausePanel.Show();
        }

        public void HideUserInterfaces(UserInterfacesType interfaces)
        {
            if(interfaces.HasFlag(UserInterfacesType.None))
                return;

            if(interfaces.HasFlag(UserInterfacesType.CoinSystem) || interfaces.HasFlag(UserInterfacesType.All))
                Currency.Hide();
            if(interfaces.HasFlag(UserInterfacesType.PauseButton) || interfaces.HasFlag(UserInterfacesType.All))
                _pauseButton.Hide();
            if(interfaces.HasFlag(UserInterfacesType.PausePanel) || interfaces.HasFlag(UserInterfacesType.All))
                _pausePanel.Hide();
        }

        public void PlaySound(string soundName)
        {
            SoundSystem.Instance.TryPlay(soundName);
        }
    }

    [Flags]
    public enum UserInterfacesType
    {
        None = 0,
        PausePanel = 1,
        PauseButton = 2,
        CoinSystem = 4,
        All = PausePanel | PauseButton | CoinSystem
    }
}
using Godot;
using StepToStep.Interface;
using StepToStep.Utils;
using System;

namespace StepToStep.Systems
{
    public partial class UserInterfaceSystem : CanvasLayer
    {
        public static UserInterfaceSystem Instance;

        [Export] public Modal Modal;
        [Export] public Currency Currency;
        [Export] private Panel _pausePanel;
        [Export] private Button _pauseButton;

        public override void _Ready()
        {
            if(Instance != null && Instance != this){
                QueueFree();
                GD.PrintErr("Instance already exists");
                return;
            }

            Instance = this;
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
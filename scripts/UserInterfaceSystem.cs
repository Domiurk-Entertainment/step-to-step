using Godot;
using StepToStep.Interface;
using StepToStep.Utils;

namespace StepToStep.Systems
{
    public partial class UserInterfaceSystem : Control
    {
        public static UserInterfaceSystem Instance;

        [Export] public Modal Modal;
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
            resumeButton.Pressed+= ResumeButtonOnPressed;
            quitButton.Pressed+= QuitButtonOnPressed;

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
    }
}
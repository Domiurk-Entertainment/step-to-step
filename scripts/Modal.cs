using Godot;
using System;

namespace StepToStep.Interface
{
    public partial class Modal : PanelContainer
    {
        [Export] private Label _title;
        [Export] private Label _content;
        [Export] private Button _actionButtonOne;
        [Export] private Button _actionButtonTwo;

        private Action _oneAction;
        private Action _twoAction;

        public void Open(string title = "",
                         string content = "",
                         string textOneAction = "OK",
                         string textTwoAction = "Cancel",
                         Action oneAction = null,
                         Action twoAction = null)
        {

            if(!string.IsNullOrEmpty(title)){
                _title.Text = title;
                _title.Show();
            }
            else{
                _title.Hide();
            }

            if(!string.IsNullOrEmpty(content)){
                _content.Text = content;
                _content.Show();
            }
            else{
                _content.Hide();
            }

            _oneAction = oneAction;
            _twoAction = twoAction;
            _actionButtonOne.Text = textOneAction;
            _actionButtonTwo.Text = textTwoAction;

            if(_oneAction != null){
                _actionButtonOne.Pressed += _oneAction;
                _actionButtonOne.Show();
            }
            else{
                _actionButtonOne.Hide();
            }

            if(_twoAction != null){
                _actionButtonTwo.Pressed += twoAction;
                _actionButtonTwo.Show();
            }
            else{
                _actionButtonTwo.Hide();
            }

            Show();
        }

        public void Close()
        {
            if(_oneAction != null){
                _actionButtonOne.Pressed -= _oneAction;
                _oneAction = null;
            }

            if(_twoAction != null){
                _actionButtonTwo.Pressed -= _twoAction;
                _twoAction = null;
            }

            Hide();
        }
    }
}
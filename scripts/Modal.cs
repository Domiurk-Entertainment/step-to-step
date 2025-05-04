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
        [Export] private Button _actionButtonClose;

        private Action _oneAction;
        private Action _twoAction;

        public void Initial(string title = "",
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

            _actionButtonOne.Text = textOneAction;
            _actionButtonTwo.Text = textTwoAction;

            if(oneAction != null){
                if(_oneAction != null)
                    _actionButtonOne.Pressed -= _oneAction;
                _oneAction = oneAction;
                _actionButtonOne.Pressed += _oneAction;
                _actionButtonOne.Show();
            }
            else{
                _actionButtonOne.Hide();
            }

            if(twoAction != null){
                if(_twoAction != null)
                    _actionButtonTwo.Pressed -= twoAction;
                _twoAction = twoAction;
                _actionButtonTwo.Pressed += twoAction;
                _actionButtonTwo.Show();
            }
            else{
                _actionButtonTwo.Hide();
            }

            _actionButtonClose.Pressed += Close;
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
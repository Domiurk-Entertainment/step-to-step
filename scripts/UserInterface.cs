using Godot;
using System;

public partial class UserInterface : Control
{
	[Export] private Control _pausePanel;
	[Export] private PackedScene _mainMenu;

	public void ShowPause()
	{
		_pausePanel.Show();
	}
	public void HidePause()
	{
		_pausePanel.Hide();
	}
}

extends Node2D

@export var map: PackedScene


func _quit_game():
	get_tree().quit()

func _play_game():
	SceneTransition.ChangeScene(map)

func _button_hover_sound():
	$"Hover Sound".play()

func _button_pressed_sound():
	$"Click Sound".play()

func _opened_settings():
	$Control/SettingsMenu.show()
	$Control/Settings.disabled = true
	$Control/VBoxContainer/Play.disabled = true
	$Control/VBoxContainer/Return.disabled = true
	$Control/VBoxContainer/Quit.disabled = true

func _closed_settings():
	$Control/SettingsMenu.hide()
	$Control/Settings.disabled = false
	$Control/VBoxContainer/Play.disabled = false
	$Control/VBoxContainer/Return.disabled = false
	$Control/VBoxContainer/Quit.disabled = false

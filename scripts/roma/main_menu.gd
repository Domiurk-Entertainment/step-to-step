extends Node2D

func _on_quit_pressed():
	$AudioStreamPlayer2D3.play()
	get_tree().quit()

func _on_play_pressed():
	$AudioStreamPlayer2D3.play()
	SceneTransition.change_scene("res://scenes/map_sprite.tscn")

func _on_play_mouse_entered():
	$AudioStreamPlayer2D2.play()

func _on_quit_mouse_entered():
	$AudioStreamPlayer2D2.play()
	
func _on_return_mouse_entered():
	$AudioStreamPlayer2D2.play()

func _on_settings_mouse_entered():
	$AudioStreamPlayer2D2.play()

func _on_settings_pressed():
	$AudioStreamPlayer2D3.play()

func _on_return_pressed():
	$AudioStreamPlayer2D3.play()

extends Node2D


func _process(delta):
	pass


func _on_quit_pressed():
	get_tree().quit()


func _on_play_pressed():
	SceneTransition.change_scene("res://Demo_Scenes/map_sprite.tscn")

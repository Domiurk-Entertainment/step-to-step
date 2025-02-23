extends CanvasLayer

func change_scene(target: String, type: String = 'enter') -> void:
	if type == 'enter':
		$AnimationPlayer.play('enter')
	await $AnimationPlayer.animation_finished
	get_tree().change_scene_to_file(target)
	$AnimationPlayer.play('exit')

extends CanvasLayer

func change_scene(target: String, type: String = 'whooot') -> void:
	if type == 'whooot':
		transition_dissolve(target)
	#else:
	#	transition_clouds(target)

func transition_dissolve(target: String) -> void:
	$AnimationPlayer.play('whooot')
	await $AnimationPlayer.animation_finished
	get_tree().change_scene_to_file(target)
	$AnimationPlayer.play_backwards('whooot')

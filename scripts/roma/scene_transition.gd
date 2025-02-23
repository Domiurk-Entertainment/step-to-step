extends CanvasLayer

var data: Dictionary = {}

func change_scene(target, type: String = 'enter'):
	if type == 'enter':
		$AnimationPlayer.play('enter')
	await $AnimationPlayer.animation_finished
	if target is PackedScene:
		get_tree().change_scene_to_packed(target)
	else:
		get_tree().change_scene_to_file(target)
	$AnimationPlayer.play('exit')

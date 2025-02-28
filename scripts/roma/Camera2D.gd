extends Camera2D


@export var shake_duration = 0.2
@export var shake_amplitude = 6.5
@export var shake_timer = 0.1
@export var original_position:Vector2

func start_shake():
	original_position = position
	shake_timer = shake_duration

func _process(delta):
	if shake_timer > 0:
		shake_timer -= delta
		if shake_timer <= 0:
			position = original_position
		else:
			position = original_position + get_random()

func get_random():
	var x = randf_range(-shake_amplitude, shake_amplitude)
	var y = randf_range(-shake_amplitude, shake_amplitude)
	return Vector2(x, y)

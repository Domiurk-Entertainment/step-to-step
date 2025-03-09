extends Camera2D

@export var shake_duration = 0.2
@export var shake_amplitude = 6.5
@export var shake_timer = 0
@export var original_position:Vector2

func start_shake():
	original_position = position
	shake_timer = shake_duration

func _process(delta):
	if shake_timer > 0:
		if shake_timer <= 0:
			position = original_position
			shake_timer = 0
		else:
			position = original_position + get_random()
		shake_timer -= delta

func get_random():
	var x = randf_range(-shake_amplitude, shake_amplitude)
	var y = randf_range(-shake_amplitude, shake_amplitude)
	return Vector2(x, y)

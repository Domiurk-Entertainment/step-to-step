extends CharacterBody2D

func _physics_process(delta):
	if velocity.x > 0:
		$PlayerAnimMenu.flip_h = false
	elif velocity.x < 0:
		$PlayerAnimMenu.flip_h = true
	if velocity.length() > 0:
		$PlayerAnimMenu.play("run")
	else:
		$PlayerAnimMenu.play("idle")

		

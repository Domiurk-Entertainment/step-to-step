extends CharacterBody2D


func _process(delta):
	if position.x == 0:
		$AnimatedSprite2D.play("idle")
	if position.y == 0:
		$AnimatedSprite2D.play("idle")

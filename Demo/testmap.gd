extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _on_att_pressed():
	$AnimationPlayer.play("AnimationAttButton")


func _on_sund_pressed():
	$AnimationPlayer.play("AnimationSund")


func _on_shop_pressed():
	$AnimationPlayer.play("AnimationShop")


func _on_att_2_pressed():
	$AnimationPlayer.play("AnimationAtt2")


func _on_att_3_pressed():
	$AnimationPlayer.play("AnimationAtt3")

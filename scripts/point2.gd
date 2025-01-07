extends Node2D

var ggL = false
var ggr= false
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
	
		
func _on_buttonbattle_left_pressed():
	$"../AnimationPlayer".play("animButtonBattleLeft")
	


func _on_buttontreasure_right_pressed():
	$"../AnimationPlayer".play("animButtonTreasure")


func _on_animation_player_animation_finished(anim_name:):
	if anim_name == "animButtonBattleLeft":
		ggL = true
	if anim_name == "animButtonBattleLeft2":
		ggr = true
	


func _on_shop_pressed():
	if ggL == true:
		$"../AnimationPlayer".play("ainimButtonShopLeft")
	else:
		$"../AnimationPlayer".play("animButtonShopRight")


func _on_buttonbattle_left_2_pressed():
	$"../AnimationPlayer".play("animButtonBattleLeft2")


func _on_buttonbattle_right_pressed():
	$"../AnimationPlayer".play("animButtonBattleRight")


func _on_button_boss_pressed():
	if ggr == true:
		$"../AnimationPlayer".play("animButtonBossLeft")
	else:
		$"../AnimationPlayer".play("animButtonBossRight")

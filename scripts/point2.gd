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
		SceneTransition.change_scene("res://scenes/sunduk.tscn")
	if anim_name == "ainimButtonShopLeft":
		SceneTransition.change_scene("res://scenes/shop.tscn")
	if anim_name == "animButtonBattleLeft":
		SceneTransition.change_scene("res://scenes/battle_left_1.tscn")
	if anim_name == "animButtonBattleRight":
		SceneTransition.change_scene("res://scenes/battleright_2.tscn")
	if anim_name == "animButtonTreasure":
		SceneTransition.change_scene("res://scenes/battle_right_1.tscn")
	if anim_name == "ainimButtonShopRight":
		SceneTransition.change_scene("res://scenes/shop.tscn")
	if anim_name == "animButtonBossLeft":
		SceneTransition.change_scene("res://scenes/battle_boss.tscn")
	if anim_name == "animButtonBossRight":
		SceneTransition.change_scene("res://scenes/battle_boss.tscn")

func _on_shop_pressed():
	if ggL == true:
		$"../AnimationPlayer".play("ainimButtonShopLeft")
	else:
		$"../AnimationPlayer".play("animButtonShopRight")


func _on_buttonbattle_right_pressed():
	$"../AnimationPlayer".play("animButtonBattleRight")


func _on_button_boss_pressed():
	if ggr == true:
		$"../AnimationPlayer".play("animButtonBossLeft")
	else:
		$"../AnimationPlayer".play("animButtonBossRight")


func _on_button_sunduk_pressed():
	$"../AnimationPlayer".play("animButtonBattleLeft2")

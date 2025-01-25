extends Node2D

@export var node_paths: Dictionary


# Called when the node enters the scene tree for the first time.
func _ready():
	var child_with_scripts:Array[Node2D]
	for child in self.get_children():
		if child.get("script") == get("script") and not check(child):
			child_with_scripts.append(child)
	for node_path in node_paths:
		var node = get_node(node_path)
		if check(node) and not node_paths[node_path] == "":
			print("played to node")
			play(node,node_paths[node_path])
	for child in child_with_scripts:
		child.request_ready()

func check(node) -> bool:
	return node is AnimatedSprite2D
	
func play(node, animation_name):
	var animation_node:AnimatedSprite2D = node as AnimatedSprite2D
	animation_node.play(animation_name)

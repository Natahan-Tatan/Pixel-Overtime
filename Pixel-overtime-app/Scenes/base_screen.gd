extends Panel

const AnimationEntry = preload("res://Scenes/horizontal_entry_animation.gd")

@export var animated_container: NodePath
@onready var animated_container_node: AnimationEntry = self.get_node(animated_container)

@export var first_focused_input: Control = null

func _ready() -> void:
	get_tree().connect("node_added", _tweak_popup_viewport)
	if(first_focused_input != null):
		await animated_container_node.animation_finished
		first_focused_input.grab_focus.call_deferred()

func _goto_screen_with_animation(scene_path) -> void:
	animated_container_node.reverse = true;
	
	if(animated_container_node.start_animation()):
		var newScene: PackedScene = ResourceLoader.load(scene_path)

		await animated_container_node.animation_finished

		self.visible = false
		get_node("/root").add_child(newScene.instantiate())

		self.queue_free()

func _tweak_popup_viewport(node: Node) -> void:
	if(node is Viewport):
		node.canvas_item_default_texture_filter = Viewport.DEFAULT_CANVAS_ITEM_TEXTURE_FILTER_NEAREST

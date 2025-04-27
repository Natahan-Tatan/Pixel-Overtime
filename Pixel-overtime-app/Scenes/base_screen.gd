extends Panel

const AnimationEntry = preload("res://Scenes/horizontal_entry_animation.gd")

@export var animated_container: NodePath
@onready var animated_container_node: AnimationEntry = self.get_node(animated_container)

@export var first_focused_input: Control = null

func _ready() -> void:
    if(first_focused_input != null):
        await animated_container_node.animation_finished
        first_focused_input.grab_focus.call_deferred()

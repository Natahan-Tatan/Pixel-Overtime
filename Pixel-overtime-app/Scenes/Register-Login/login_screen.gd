extends "res://Scenes/base_screen.gd"

@export_group("Screens")
@export_file("*.tscn") var register: String


func _on_register_button_pressed() -> void:
	animated_container_node.reverse = true;
	
	if(animated_container_node.start_animation()):
		var newScene: PackedScene = ResourceLoader.load(register)

		await animated_container_node.animation_finished

		self.visible = false
		get_node("/root").add_child(newScene.instantiate())

		self.queue_free()

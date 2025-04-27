extends "res://Scenes/base_screen.gd"

@export_group("Screens")
@export_file("*.tscn") var login: String

func _on_log_in_button_pressed() -> void:
	animated_container_node.reverse = true;
	animated_container_node.start_animation()

	var newScene: PackedScene = ResourceLoader.load(login)

	await animated_container_node.animation_finished

	self.visible = false
	get_node("/root").add_child(newScene.instantiate())

	self.queue_free()

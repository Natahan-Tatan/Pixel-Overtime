extends "res://Scenes/base_screen.gd"

@export_group("Screens")
@export_file("*.tscn") var login: String

func _on_log_in_button_pressed() -> void:
	self._goto_screen_with_animation(login)

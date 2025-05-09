extends "res://Scenes/base_screen.gd"

@export_group("Screens")
@export_file("*.tscn") var register: String
@export_file("*.tscn") var settings: String


func _on_register_button_pressed() -> void:
	self._goto_screen_with_animation(register)


func _on_settings_button_pressed() -> void:
	self._goto_screen_with_animation(settings)

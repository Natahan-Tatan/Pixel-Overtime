extends "res://Scenes/base_screen.gd"

var WebService = preload("res://Singletons/WebService/web_service.gd")
var FormField = preload("res://Prefabs/FormField/form_field.gd")

@export_group("Screens")
@export_file("*.tscn") var register: String
@export_file("*.tscn") var settings: String
@export_file("*.tscn") var dashboard: String

@onready var webservice:= $/root/WebService as WebService

func _on_register_button_pressed() -> void:
	self._goto_screen_with_animation(register)


func _on_settings_button_pressed() -> void:
	self._goto_screen_with_animation(settings)

func _on_log_in_button_pressed() -> void:
	%GlobalErrorLabel.visible = false
	var errors = await webservice.login(%EmailField.value, %PasswordField.value)

	if(errors.size() > 0):
		for field in get_tree().get_nodes_in_group("Form Fields"):
			if(field.get_script() == FormField):
				field.display_error(errors)
	
		if(errors.has("") and errors[""] != ""):
			%GlobalErrorLabel.text = errors[""]
			%GlobalErrorLabel.visible = true
	else:
		self._goto_screen_with_animation(dashboard)

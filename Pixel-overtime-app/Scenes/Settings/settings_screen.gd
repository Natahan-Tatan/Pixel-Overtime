extends "res://Scenes/base_screen.gd"

var ConfigValuesScript = preload("res://Singletons/Config/config_values.gd")

@export_group("Screens")
@export_file("*.tscn") var login: String

@onready var config_singleton:= $"/root/Config" as Config
@onready var scale_option:= $"%ScaleOptionButton" as OptionButton
@onready var instance_option:= $"%InstanceOptionButton" as OptionButton
@onready var custom_instance_container = $"%CustomInstanceField" as Control
@onready var custom_instance_edit = $"%CustomInstanceEdit" as LineEdit

@onready var custom_instance_container_tween: Tween

func _on_return_button_pressed() -> void:
	self._goto_screen_with_animation(login)

func _ready() -> void:
	super._ready()

	scale_option.selected = scale_option.get_item_index(int(config_singleton.values.ui_scale * 100))

	instance_option.selected = instance_option.get_item_index(config_singleton.values.instance_type)

	if(config_singleton.values.instance_type != ConfigValuesScript.InstanceType.CUSTOM):
		for child in custom_instance_container.get_children():
			if(child is Control):
				child.modulate = Color.TRANSPARENT
	else:
		for child in custom_instance_container.get_children():
			if(child is Control):
				child.modulate = Color.WHITE

	custom_instance_edit.text = config_singleton.values.custom_instance_url

func _on_scale_option_button_item_selected(index:int) -> void:
	var text_id = scale_option.get_item_id(index)
	config_singleton.values.ui_scale = float(text_id) / 100.0
	config_singleton.save()

func _on_instance_option_button_item_selected(index:int) -> void:
	config_singleton.values.instance_type = instance_option.get_item_id(index)

	if(custom_instance_container_tween != null):
		custom_instance_container_tween.stop()

	custom_instance_container_tween = get_tree().create_tween()

	if(config_singleton.values.instance_type != ConfigValuesScript.InstanceType.CUSTOM):
		$"%ReturnButton".disabled = false
		for child in custom_instance_container.get_children():
			if(child is Control):
				custom_instance_container_tween.parallel().tween_property(child, "modulate", Color.TRANSPARENT, 0.15)
	else:
		self._update_instance_url(custom_instance_edit.text)
		for child in custom_instance_container.get_children():
			if(child is Control):
				custom_instance_container_tween.parallel().tween_property(child, "modulate", Color.WHITE, 0.15)

	config_singleton.save()


func _update_instance_url(url:String) -> void:
	var regex = RegEx.new()
	regex.compile("^https?://[^\\s]+$")

	if(regex.search(url) != null):
		config_singleton.values.custom_instance_url = url
		config_singleton.save()
		$"%CustomInstanceEditErrorLabel".visible = false
		$"%ReturnButton".disabled = false
	else:
		$"%ReturnButton".disabled = true
		$"%CustomInstanceEditErrorLabel".visible = true

func _on_custom_instance_edit_text_changed(new_text: String) -> void:
	self._update_instance_url(new_text)

@tool
extends VBoxContainer

## Keep empty to use node name
@export var field_name: String:
	get:
		return _field_name
	set(value):
		_field_name = value
		_compute_field_name()

var _field_name:= ""

## Label as displayed as LineEdit placeholder
@export var label: String:
	get:
		return _label
	set(value):
		_label = value
		if(line_edit != null):
			line_edit.placeholder_text = _label

var _label:= ""

## Label as displayed as LineEdit placeholder
@export var secret: bool:
	get:
		return _secret
	set(value):
		_secret = value
		if(line_edit != null):
			line_edit.secret = _secret

var _secret:= false

var value: String:
	get:
		return line_edit.text 

@onready var line_edit:= $LineEdit as LineEdit
@onready var error_label:= $ErrorContainer/ErrorLabel as Label

func _ready() -> void:
	_compute_field_name()
	line_edit.placeholder_text = _label
	line_edit.secret = _secret

func _compute_field_name():
	if(_field_name == null or _field_name == ""):
		_field_name = self.name.substr(0, self.name.length()-5).to_lower() if self.name.ends_with("Field") else self.name.to_lower()

func display_error(errors: Dictionary) -> void:
	error_label.visible = false
	for key in errors:
		if(key == field_name && errors[key] != ""):
			error_label.text = errors[key]
			error_label.visible = true

func hide_error() -> void:
	error_label.visible = false
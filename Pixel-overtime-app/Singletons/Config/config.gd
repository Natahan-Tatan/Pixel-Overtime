extends Node

const CONFIG_FILE: = "user://config.tres"

var _values: ConfigValues = null

@export var values: ConfigValues:
	get:
		if(_values == null):
			if(ResourceLoader.exists(CONFIG_FILE)):
				_values = ResourceLoader.load(CONFIG_FILE) as ConfigValues

			if(_values == null):
				DirAccess.remove_absolute(CONFIG_FILE)

				var newValues = ConfigValues.new()

				# Set computed default settings value here
				newValues.ui_scale = DisplayServer.screen_get_scale()

				values = newValues

		return _values
	set(value):
		_values = value
		if(_is_ready):
			_update_settings()
		ResourceSaver.save(_values, CONFIG_FILE)

var _is_ready:= false
func _ready() -> void:

	_update_settings()
	_is_ready = true

func _update_settings() -> void:
	print(values.ui_scale)
	get_tree().root.content_scale_factor = float(ProjectSettings.get_setting("display/window/stretch/scale")) * values.ui_scale

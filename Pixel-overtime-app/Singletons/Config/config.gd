extends Node

var WebService = preload("res://Singletons/WebService/web_service.gd")

const CONFIG_FILE: = "user://config.tres"
const OFFICIAL_INSTANCE: = "https://api.flovertime.natahan.net"

var _values: ConfigValues = null

@onready var web_service_node:= $"/root/WebService" as WebService

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
			_apply_settings()
		ResourceSaver.save(_values, CONFIG_FILE)

var _is_ready:= false
func _ready() -> void:
	_apply_settings()
	_is_ready = true

func save() -> void:
	values = _values

func _apply_settings() -> void:
	get_tree().root.content_scale_factor = float(ProjectSettings.get_setting("display/window/stretch/scale")) * values.ui_scale

	if(values.instance_type == ConfigValues.InstanceType.CUSTOM):
		web_service_node.instance_host = values.custom_instance_url
	else:
		web_service_node.instance_host = OFFICIAL_INSTANCE

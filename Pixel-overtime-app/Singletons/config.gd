extends Node

func _ready() -> void:
    print("Seetings: screen scale is %f" % DisplayServer.screen_get_scale())
    get_tree().root.content_scale_factor = float(ProjectSettings.get_setting("display/window/stretch/scale")) * DisplayServer.screen_get_scale()
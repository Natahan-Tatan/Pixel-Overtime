extends MenuButton

var WebService = preload("res://Singletons/WebService/web_service.gd")

@onready var webservice:= $/root/WebService as WebService

func _ready() -> void:
    get_popup().connect("id_pressed", _on_menu_button_pressed)

func _on_menu_button_pressed(id: int):
    webservice.logout()

extends "res://Scenes/base_screen.gd"

var WebService = preload("res://Singletons/WebService/web_service.gd")

@onready var webservice:= $/root/WebService as WebService

@export_group("Screens")
@export_file("*.tscn") var login: String

func _ready() -> void:
    webservice.connect("user_logout",on_user_logout)

func on_user_logout():
    self._goto_screen_with_animation(login)
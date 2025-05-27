extends "res://Scenes/base_screen.gd"

var WebService = preload("res://Singletons/WebService/web_service.gd")
var Config = preload("res://Singletons/Config/config.gd")

@onready var webservice:= $/root/WebService as WebService
@onready var config:= $/root/Config as Config

@export var small_width:= 1200

@export_group("Screens")
@export_file("*.tscn") var login: String

func _ready() -> void:
	super._ready()
	webservice.connect("user_logout",on_user_logout)

	self._on_viewport_size_changed()
	get_viewport().connect("size_changed", self._on_viewport_size_changed)

func on_user_logout():
	self._goto_screen_with_animation(login)

func _on_viewport_size_changed() -> void:
	print(get_viewport_rect().size.x, " ", small_width)
	if(get_viewport_rect().size.x < small_width):
		$ScrollContainer/CenterContainer/AutoMarginContainer/MainContainer/ContentContainer.vertical = true
	else:
		$ScrollContainer/CenterContainer/AutoMarginContainer/MainContainer/ContentContainer.vertical = false

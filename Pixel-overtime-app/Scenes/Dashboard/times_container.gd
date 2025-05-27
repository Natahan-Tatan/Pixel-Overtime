extends VBoxContainer

var WebService = preload("res://Singletons/WebService/web_service.gd")

@onready var webservice:= $/root/WebService as WebService

func _ready() -> void:
	self.Update()

func Update() -> void:
	var times = await webservice.get_times_list()
	

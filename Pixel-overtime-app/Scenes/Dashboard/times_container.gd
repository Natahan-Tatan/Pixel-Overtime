extends VBoxContainer

var WebService = preload("res://Singletons/WebService/web_service.gd")
var TimeItem = preload("res://Prefabs/Dashboard/time.gd")

@onready var webservice:= $/root/WebService as WebService

@export var timeItemScene:= preload("res://Prefabs/Dashboard/time.tscn") as PackedScene

func _ready() -> void:
	self.Update()

func Update() -> void:
	var times = await webservice.get_times_list()

	for child in get_children():
		self.remove_child(child)
		child.queue_free()

	for time in times:
		var item = timeItemScene.instantiate()
		item.time = Api_Time.new(time)
		self.add_child(item)
		


	

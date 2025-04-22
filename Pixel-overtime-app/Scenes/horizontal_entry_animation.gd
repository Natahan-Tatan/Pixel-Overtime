extends Container

@export var firstComeFromRight:= false
@export var duration:= 1.0
@export var elements: Array[Control]

var elements_position: Dictionary

@onready var screen_rect:= get_viewport_rect()
@onready var tween:= get_tree().create_tween()

func _replace_tree() -> void:
	for el in elements:
		elements_position[el] = el.position

	var newControl = Control.new()
	newControl.position = self.position

	self.replace_by(newControl)
	_set_tween()

	await tween.finished

	newControl.replace_by(self)
	newControl.queue_free()

func _ready() -> void:
	get_viewport().connect("size_changed", self._on_viewport_size_changed)
	call_deferred("_replace_tree")

func _on_viewport_size_changed():
	print("Screen resized: ", get_viewport_rect())
	screen_rect = get_viewport_rect()

func _set_tween() -> void:
	var right = firstComeFromRight
	for el in elements:
		if(right):
			el.global_position = Vector2(screen_rect.size.x, el.global_position.y)
		else:
			el.global_position = Vector2(-el.get_global_rect().size.x, el.global_position.y)
		print(el.global_position)
		right = !right

		tween.parallel().tween_property(el, "position", elements_position[el], duration)

# func _process(delta: float) -> void:
#     _set_tween()

extends Container

signal animation_finished()

@export var firstComeFromRight:= false
@export var duration:= 1.0
@export var reverse:= false
@export var elements: Array[Control]

var elements_position: Dictionary

@onready var screen_rect:= get_viewport_rect()
@onready var tween: Tween

func start_animation() -> void:
	for el in elements:
		elements_position[el] = el.position

	var newControl = Control.new()
	newControl.position = self.position

	tween = get_tree().create_tween();
	self.replace_by(newControl)
	
	var right = firstComeFromRight
	for el in elements:
		var remote_position:= Vector2.ZERO
		if(right):
			remote_position = Vector2(screen_rect.size.x, el.global_position.y)
		else:
			remote_position = Vector2(-el.get_global_rect().size.x, el.global_position.y)

		if(!reverse):
			el.global_position = remote_position;
			tween.parallel().tween_property(el, "position", elements_position[el], duration)
		else:
			el.global_position = elements_position[el]
			tween.parallel().tween_property(el, "position", remote_position, duration)

		right = !right

	await tween.finished

	newControl.replace_by(self)
	newControl.queue_free()

	emit_signal("animation_finished")

func _ready() -> void:
	get_viewport().connect("size_changed", self._on_viewport_size_changed)
	call_deferred("start_animation")

func _on_viewport_size_changed():
	screen_rect = get_viewport_rect()

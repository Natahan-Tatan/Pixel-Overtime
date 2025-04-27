class_name horizontal_entry_animation

extends Container

signal animation_finished()

@export var firstComeFromRight:= false
@export var duration:= 1.0
@export var reverse:= false
@export var elements: Array[Control]

var elements_position: Dictionary

@onready var screen_size = get_viewport().size;
@onready var tween: Tween

func start_animation() -> bool:

	if(tween != null and tween.is_running()):
		return false

	for el in elements:
		elements_position[el] = el.global_position

	var newControl = Control.new()
	newControl.global_position = self.global_position

	tween = get_tree().create_tween()
	tween.finished.connect(self._on_tween_finished.bind(newControl), CONNECT_ONE_SHOT)

	screen_size = get_viewport().get_visible_rect().size
	
	var right = firstComeFromRight
	for el in elements:
		el.modulate = Color.WHITE
		var remote_position:= Vector2.ZERO
		if(right):
			remote_position = Vector2(screen_size.x * 1.3, el.global_position.y)
		else:
			remote_position = Vector2(-el.get_global_rect().size.x, el.global_position.y)

		if(!reverse):
			el.global_position = remote_position;
			tween.parallel().tween_property(el, "global_position", elements_position[el], duration)
		else:
			el.global_position = elements_position[el]
			tween.parallel().tween_property(el, "global_position", remote_position, duration)

		right = !right

	self.replace_by(newControl)

	return true

func _on_tween_finished(newControl: Control) -> void:
	newControl.replace_by(self)
	newControl.queue_free()

	emit_signal("animation_finished")

func _ready() -> void:
	for el in elements:
		el.modulate = Color.TRANSPARENT

	await get_tree().process_frame
	await get_tree().process_frame

	start_animation()

@tool
extends TextEdit

class_name FlovertimeTextEdit

static var _disable_normal_stylebox:= preload("res://FlovertimeTheme/Widgets/LineEdit/TextEdit/text_edit_static.stylebox")

func _ready() -> void:
	self._set("editable", self.editable)
	self.clip_contents = false;
	self.wrap_mode = TextEdit.LINE_WRAPPING_BOUNDARY

func _set(property: StringName, value: Variant) -> bool:
	if(property == "editable"):
		if(value):
			super.set("theme_override_styles/normal", null)
		else:
			super.set("theme_override_styles/normal", _disable_normal_stylebox)

	return false

func _gui_input(event: InputEvent) -> void:
	if(event.is_action_pressed("ui_focus_next")):
		self.find_next_valid_focus().grab_focus()
		if(self.editable):
			self.editable = false
			call_deferred("_on_remove_readonly")
	elif(event.is_action_pressed("ui_focus_prev")):
		self.find_prev_valid_focus().grab_focus()
		if(self.editable):
			self.editable = false
			call_deferred("_on_remove_readonly")

func _on_remove_readonly():
	self.editable = true
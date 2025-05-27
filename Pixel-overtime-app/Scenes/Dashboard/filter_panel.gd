extends PanelContainer

func _process(_delta: float) -> void:
	self.global_position = Vector2(self.global_position.x, 0)

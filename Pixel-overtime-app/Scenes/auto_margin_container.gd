extends MarginContainer

@export_range(10, 100) var max_viewport_percent:= 80;
@export_range(100, 1500) var min_width:= 680;
@export_range(100, 5000) var max_width:= 1280;

func _ready() -> void:
    get_viewport().connect("size_changed", self._on_viewport_size_changed)

func _on_viewport_size_changed() -> void:
    var new_width = clamp(get_viewport_rect().size.x * (float(self.max_viewport_percent)/100.0), min_width, max_width)

    self.custom_minimum_size = Vector2(new_width, self.custom_minimum_size.y)
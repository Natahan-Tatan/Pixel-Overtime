extends PanelContainer

var time: Api_Time:
    set(value):
        _time = value
        if(_is_ready):
            self._update_from_time()
    get:
        return _time
var _time: Api_Time

var _is_ready:= false

func _ready() -> void:
    self._update_from_time()
    _is_ready = true

func _update_from_time():
    $HBoxContainer/DateLabel.text = "%s/%s/%s" % [_time.date["year"], _time.date["month"], _time.date["day"]]
    $HBoxContainer/DescriptionLabel.text = _time.description

extends Resource

class_name ConfigValues

@export_range(0.25,5,0.25) var ui_scale:= 1.0

enum InstanceType
{
    OFFICIAL,
    CUSTOM
}
@export_enum("InstanceType") var instance_type:= 0

@export var custom_instance_url := ""
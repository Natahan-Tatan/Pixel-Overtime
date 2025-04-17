extends Resource

class_name User

@export var id = ""
@export var name = ""
@export var email = ""
@export var email_confirmed = false
@export var account_created_date: Dictionary
@export var last_check_time: float

@export var bearer = ""
@export var refresh = ""
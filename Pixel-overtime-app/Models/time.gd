extends Resource

class_name Api_Time

enum Api_Time_Type
{
    OVERTIME,
    CATCH_UP
}

enum Api_Time_Reason
{
    UNSPECIFIED,
    FIX_BUG,
    DEPLOYMENT,
    MEETING
}

@export var id: String
@export var user_id: String
@export var time_type: Api_Time_Type
@export var time_reason: Api_Time_Reason
@export var date: Dictionary
@export var duration_minutes: int
@export var description: String
@export var created_at: Dictionary


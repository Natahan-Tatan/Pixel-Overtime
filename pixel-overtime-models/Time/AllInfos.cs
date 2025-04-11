using System;

namespace pixel_overtime_models.Time;

public class AllInfos
{
    public string Id {get;set;} = "";
    public string UserId {get;set;} = "";
    public TimeType TimeType {get;set;}
    public TimeReason TimeReason {get;set;}
    public DateTime Date {get;set;}
    public int DurationMinutes {get;set;}
    public string Description {get;set;} = "";
    public DateTime CreateAt {get;set;}
}

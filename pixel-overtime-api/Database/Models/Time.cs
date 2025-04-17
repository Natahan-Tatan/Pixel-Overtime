using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pixel_overtime_api.Database.Models;

public class Time
{
    public Time()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Key]
    public string Id {get;set;}

    [ForeignKey(nameof(UserId))]
    public User? User {get;set;}
    public string UserId {get;set;} = "";

    public pixel_overtime_models.Time.TimeType TimeType {get;set;}
    public pixel_overtime_models.Time.TimeReason TimeReason {get;set;} = pixel_overtime_models.Time.TimeReason.UNSPECIFIED;
    public DateTime Date {get;set;}
    public int DurationMinutes {get;set;} = 0;
    public string Description {get;set;} = "";

    public DateTime CreateAt {get;set;} = DateTime.UtcNow;
}

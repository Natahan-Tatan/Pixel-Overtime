using System;
using System.ComponentModel.DataAnnotations;

namespace pixel_overtime_models.Time;

/// <summary>
/// Used by TimeController to add a new time to the current connected user
/// </summary>
public class AddTime
{
    /// <summary>
    /// Type of time (OVERTIME, CATCH_UP)
    /// </summary>
    /// <example>OVERTIME</example>
    [Required]
    public TimeType TimeType {get;set;}

    /// <summary>
    /// Reason of the overtime
    /// </summary>
    /// <example>FIX_BUG</example>
    [Required]
    public TimeReason TimeReason {get;set;} = TimeReason.UNSPECIFIED;
    
    /// <summary>
    /// Date when this time occurred
    /// </summary>
    /// <example>2025-04-11T15:07:01.148Z</example>
    [Required]
    public DateTime Date {get;set;}

    /// <summary>
    /// Duration of overtime or catch-up in minutes
    /// </summary>
    /// <example>45</example>
    [Required]
    public int DurationMinutes {get;set;}

    /// <summary>
    /// Description of the time
    /// </summary>
    /// <example>Fatal error in application.</example>
    public string Description {get;set;} = "";
}

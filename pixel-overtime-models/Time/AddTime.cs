//  
//   Pixel Overtime API
//   Copyright (C) 2025  Natahan Tatan <license@natahan.net>
//   
//   This software is provided free of charge for personal, non-commercial, and non-professional use only.
//   
//   Permissions
//   You are permitted to:
//   Use this software for personal and educational purposes.
//   Modify the source code for personal use only.
//   Share the unmodified version with others for personal use only, as long as this license file is included.
//   
//   Restrictions
//   You are not permitted to:
//   Use this software in any commercial, professional, or for-profit context.
//   Sell, sublicense, or distribute this software as part of any paid service or product.
//   Use this software within an organization or business.
//   Modify and redistribute the software, unless with the express written permission of the author.
//   
//   Disclaimer
//   This software is provided "as is", without warranty of any kind, express or implied,
//   including but not limited to the warranties of merchantability, fitness for a particular purpose,
//   and noninfringement. In no event shall the authors or copyright holders
//   be liable for any claim, damages or other liability, whether in an action of contract,
//   tort or otherwise, arising from, out of or in connection with the software or the use
//   or other dealings in the software.
//   
//   ---
//   
//   If you wish to use this software for commercial or professional purposes, please contact the author to discuss licensing options.

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

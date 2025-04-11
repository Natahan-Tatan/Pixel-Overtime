using System;
using System.ComponentModel.DataAnnotations;

namespace pixel_overtime_models.Me;

public class SetInfos
{
    [Required]
    [StringLength(100, MinimumLength = 2)] 
    public string Name {get;set;} = "";
}

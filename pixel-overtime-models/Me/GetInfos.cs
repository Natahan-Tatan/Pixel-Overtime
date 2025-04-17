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

namespace pixel_overtime_models.Me;

/// <summary>
/// Classe used by MeController to send infos to user about itself
/// </summary>
public class GetInfos
{
    /// <summary>
    /// Id of the user
    /// </summary>
    /// <example>647d-d392-4434-a8da-2303e</example>
    public string Id {get;set;} = "";

    /// <summary>
    /// Email of the user
    /// </summary>
    /// <example>example@example.com</example>
    public string Email {get;set;} = "";

    /// <summary>
    /// Is email confirmed
    /// </summary>
    /// <example>true</example>
    public bool EmailConfirmed {get;set;} = false;

    /// <summary>
    /// Displayed name of the user
    /// </summary>
    /// <example>René Buisson</example>
    public string Name {get;set;} = "";

    /// <summary>
    /// Creation date of the account
    /// </summary>
    /// <example>2025-04-11T13:44:31.393Z</example>
    public DateTime AccountCreatedAt {get;set;}
}

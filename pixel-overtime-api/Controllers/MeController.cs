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
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pixel_overtime_api.Database;
using pixel_overtime_api.Database.Models;


namespace pixel_overtime_api.Controllers
{
    [ApiController]
    [Authorize]
    public class MeController : ControllerBase
    {
        protected readonly UserManager<User> _userManager;
        protected readonly ApiDbContext _dbContext;

        public MeController(UserManager<User> userManager, ApiDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get infos of current connected user
        /// </summary>
        [ProducesResponseType<pixel_overtime_models.Me.GetInfos>(200)]
        [Produces("application/json")]
        [Route("/Me")]
        [HttpGet]
        public async Task<IActionResult> GetInfos()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if(user is null)
            {
                return Unauthorized();
            }

            return new ObjectResult(new pixel_overtime_models.Me.GetInfos(){
                Id = user.Id,
                Name = user.Name,
                Email = user.Email ?? "",
                EmailConfirmed = user.EmailConfirmed,
                AccountCreatedAt = user.AccountCreatedAt
            });
        }

        [HttpPatch]
        [Route("/Me")]
        public async Task<IActionResult> SetInfos([FromBody] pixel_overtime_models.Me.SetInfos infos)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if(user is null)
            {
                return Unauthorized();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(_dbContext.Users.Any(u => u.Name == infos.Name.Trim() && u.Id != user.Id))
            {
                return Conflict("Name already in use.");
            }

            user.Name = infos.Name.Trim();

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}

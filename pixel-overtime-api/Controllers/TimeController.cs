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

using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pixel_overtime_api.Database;
using pixel_overtime_api.Database.Models;
using pixel_overtime_models.Time;

namespace pixel_overtime_api.Controllers
{
    [ApiController]
    [Authorize]
    public class TimeController : ControllerBase
    {
        protected readonly UserManager<User> _userManager;
        protected readonly ApiDbContext _dbContext;

        public TimeController(UserManager<User> userManager, ApiDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [Route("/Time")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] pixel_overtime_models.Time.AddTime time)
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

            _dbContext.Times.Add(new Time(){
                User = user,
                TimeType = time.TimeType,
                Date = time.Date.Date,
                Description = time.Description,
                TimeReason = time.TimeReason,
                DurationMinutes = time.DurationMinutes
            });

            await _dbContext.SaveChangesAsync();

            return Created();
        }

        [HttpGet]
        [Route("/Time")]
        public async Task<IActionResult> GetList(
            [FromQuery] TimeType? type,
            [FromQuery] TimeReason? reason,
            [FromQuery] int? page,
            [FromQuery] int? limit
        )
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if(user is null)
            {
                return Unauthorized();
            }

            var query = _dbContext.Times
                .Where(t => t.UserId == user.Id)
                .Where(t => type == null || t.TimeType == type)
                .Where(t => reason == null || t.TimeReason == reason);

            if(page >= 0 && limit >= 1)
            {
                query = query.Skip(page.Value * limit.Value).Take(limit.Value);
            }

            return new ObjectResult(
                query
                //TODO: return light version
                .Select(t => new pixel_overtime_models.Time.AllInfos(){
                    Id = t.Id,
                    UserId = t.UserId,
                    TimeType = t.TimeType,
                    TimeReason = t.TimeReason,
                    DurationMinutes = t.DurationMinutes,
                    Description = t.Description,
                    CreateAt = t.CreateAt,
                    Date = t.Date
                })
            );
        }

        [HttpGet]
        [Route("/Time/{id}")]
        public async Task<IActionResult> Single([FromRoute]string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if(user is null)
            {
                return Unauthorized();
            }

            // We consider the attempt to access to a not owned time as a not found error to avoid ID leak
            var time = await _dbContext.Times.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);

            if(time is null)
            {
                return NotFound("Time not found");
            }

            return new ObjectResult(new pixel_overtime_models.Time.AllInfos(){
                Id = time.Id,
                UserId = time.UserId,
                TimeType = time.TimeType,
                TimeReason = time.TimeReason,
                DurationMinutes = time.DurationMinutes,
                Description = time.Description,
                CreateAt = time.CreateAt,
                Date = time.Date
            });
        }

        [HttpDelete]
        [Route("/Time/{id}")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if(user is null)
            {
                return Unauthorized();
            }

            // We consider the attempt to access to a not owned time as a not found error to avoid ID leak
            var time = await _dbContext.Times.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);

            if(time is null)
            {
                return NotFound("Time not found");
            }

            _dbContext.Times.Remove(time);

             await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch]
        [Route("/Time/{id}")]
        public async Task<IActionResult> UpdateInfos([FromRoute]string id, [FromBody] pixel_overtime_models.Time.UpdateInfos infos)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if(user is null)
            {
                return Unauthorized();
            }

            // We consider the attempt to access to a not owned time as a not found error to avoid ID leak
            var time = await _dbContext.Times.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);

            if(time is null)
            {
                return NotFound("Time not found");
            }

            time.Date = infos.Date;
            time.TimeReason = infos.TimeReason;
            time.TimeType = infos.TimeType;
            time.Description = infos.Description;
            time.DurationMinutes = infos.DurationMinutes;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}

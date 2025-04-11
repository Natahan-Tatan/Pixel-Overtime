using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

            return NoContent();
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
    }
}

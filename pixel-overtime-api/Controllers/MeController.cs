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

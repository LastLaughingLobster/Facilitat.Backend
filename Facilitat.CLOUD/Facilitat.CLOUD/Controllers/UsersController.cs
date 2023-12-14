using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Facilitat.CLOUD.Models.DTOs;
using Facilitat.CLOUD.Services.Users;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Facilitat.CLOUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> Get(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpGet("email/{email}")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
    }
}

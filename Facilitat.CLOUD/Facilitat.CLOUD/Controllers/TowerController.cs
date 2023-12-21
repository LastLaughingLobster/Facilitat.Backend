using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Facilitat.CLOUD.Models.DTOs;
using Facilitat.CLOUD.Services.Towers; 
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Facilitat.CLOUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TowerController : ControllerBase
    {
        private readonly ITowerService _towerService;

        public TowerController(ITowerService towerService)
        {
            _towerService = towerService;
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TowerDTO>>> GetTowersByUserId(int userId)
        {
            return Ok(await _towerService.GetTowersByUserId(userId));
        }

    }
}

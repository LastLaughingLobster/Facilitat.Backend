using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Facilitat.CLOUD.Models.DTOs;
using Facilitat.CLOUD.Services.Schedule;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Facilitat.CLOUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleOrderService;

        public ScheduleController(IScheduleService scheduleOrderService)
        {
            _scheduleOrderService = scheduleOrderService;
        }

        [HttpGet("tower/{towerId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ScheduleOrderDTO>>> GetAllByTower(int towerId)
        {
            return Ok(await _scheduleOrderService.GetAllByTowerAsync(towerId));
        }

        [HttpGet("towerForGrid/{towerId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GridScheduleDTO>>> GetAllByTowerForGrid(int towerId)
        {
            return Ok(await _scheduleOrderService.GetAllByTowerForGridAsync(towerId));
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ScheduleOrderDTO>>> GetAllByUser(int userId)
        {
            return Ok(await _scheduleOrderService.GetAllByUserAsync(userId));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _scheduleOrderService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create(ScheduleOrderDTO newSchedule)
        {
            var success = await _scheduleOrderService.CreateAsync(newSchedule);
            if (!success)
            {
                return Ok();
            }
            return NoContent();
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> Update(ScheduleOrderDTO newSchedule)
        {
            var success = await _scheduleOrderService.UpdateAsync(newSchedule);
            if (!success)
            {
                return Ok();
            }
            return NoContent();
        }
    }
}

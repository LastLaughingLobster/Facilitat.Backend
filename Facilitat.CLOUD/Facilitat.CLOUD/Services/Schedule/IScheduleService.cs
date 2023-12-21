using Facilitat.CLOUD.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facilitat.CLOUD.Services.Schedule
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleOrderDTO>> GetAllByTowerAsync(int towerId);
        Task<IEnumerable<GridScheduleDTO>> GetAllByTowerForGridAsync(int towerId);
        Task<IEnumerable<ScheduleOrderDTO>> GetAllByUserAsync(int userId);
        Task<bool> DeleteAsync(int id);
        Task<bool> CreateAsync(ScheduleOrderDTO newSchedule);
        Task<bool> UpdateAsync(ScheduleOrderDTO newSchedule);
    }
}

using Facilitat.CLOUD.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facilitat.CLOUD.Services.Schedule
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleOrderDTO>> GetAllByTowerAsync(int towerId);
        Task<IEnumerable<ScheduleOrderDTO>> GetAllByUserAsync(int userId);
        Task<bool> DeleteAsync(int id);
        // Add other service methods here
    }
}

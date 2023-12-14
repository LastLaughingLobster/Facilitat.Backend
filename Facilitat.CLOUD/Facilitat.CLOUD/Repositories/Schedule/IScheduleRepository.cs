using Facilitat.CLOUD.Models.Entities;
using Facilitat.CLOUD.Models.Enums;
using Facilitat.CLOUD.Repositories.Generic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facilitat.CLOUD.Repositories.Schedule
{
    public interface IScheduleRepository : IGenericRepository<ScheduleOrder>
    {
        Task<IEnumerable<ScheduleOrder>> GetByUserAsync(int userId);
        Task<IEnumerable<ScheduleOrder>> GetByTowerAsync(int towerId);
        Task<bool> UpdateScheduleOrderAsync(int scheduleOrderId, ScheduleStatus status);
    }
}

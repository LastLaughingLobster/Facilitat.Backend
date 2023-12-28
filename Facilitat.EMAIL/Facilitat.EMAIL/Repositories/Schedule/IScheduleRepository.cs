using Facilitat.EMAIL.Models.DTOs;
using Facilitat.EMAIL.Models.Entities;
using Facilitat.EMAIL.Repositories.Generic;
using System.Threading.Tasks;

namespace Facilitat.EMAIL.Repositories.Schedule
{
    public interface IScheduleRepository : IGenericRepository<ScheduleOrder>
    {
        Task<ScheduleEmailDTO> GetScheduleEmailDetailsAsync(ScheduleOrderDTO scheduleOrderDto);
    }
}


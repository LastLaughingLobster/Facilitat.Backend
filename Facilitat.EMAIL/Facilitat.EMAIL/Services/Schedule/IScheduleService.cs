using Facilitat.EMAIL.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facilitat.EMAIL.Services.Schedule
{
    public interface IScheduleService
    {
        Task ProcessScheduleOrderAsync(ScheduleOrderDTO scheduleOrderDto);
    }
}

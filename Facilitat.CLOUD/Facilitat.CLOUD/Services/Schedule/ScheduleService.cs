using Facilitat.CLOUD.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Facilitat.CLOUD.Repositories.Schedule;
using System.Linq;
using Facilitat.CLOUD.Repositories.Schedule;
using Facilitat.CLOUD.Models.Enums;

namespace Facilitat.CLOUD.Services.Schedule
{
    public class ScheduleOrderService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleOrderRepository;

        public ScheduleOrderService(IScheduleRepository scheduleOrderRepository)
        {
            _scheduleOrderRepository = scheduleOrderRepository;
        }

        public async Task<IEnumerable<ScheduleOrderDTO>> GetAllByTowerAsync(int towerId)
        {
            var scheduleOrders = await _scheduleOrderRepository.GetByTowerAsync(towerId);
            return scheduleOrders.Select(order => new ScheduleOrderDTO
            {
                // Map ScheduleOrder entity to ScheduleOrderDTO
            }).ToList();
        }

        public async Task<IEnumerable<ScheduleOrderDTO>> GetAllByUserAsync(int userId)
        {
            var scheduleOrders = await _scheduleOrderRepository.GetByUserAsync(userId);
            return scheduleOrders.Select(order => new ScheduleOrderDTO
            {
                // Map ScheduleOrder entity to ScheduleOrderDTO
            }).ToList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _scheduleOrderRepository.UpdateScheduleOrderAsync(id, ScheduleStatus.Canceled);
        }

        // Implement other service methods as needed
    }
}

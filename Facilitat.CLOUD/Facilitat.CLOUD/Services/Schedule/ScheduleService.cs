using Facilitat.CLOUD.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Facilitat.CLOUD.Repositories.Schedule;
using System.Linq;
using Facilitat.CLOUD.Models.Enums;
using Facilitat.CLOUD.Models.Entities;
using System;

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
            return scheduleOrders.Select(order => (ScheduleOrderDTO)order).ToList();
        }

        public async Task<IEnumerable<GridScheduleDTO>> GetAllByTowerForGridAsync(int towerId)
        {
            var gridSchedules = await _scheduleOrderRepository.GetByTowerForGridAsync(towerId);

            int previusApartment = 0;
            int appointmentOrder = 1;
            int apartmentCounter = 0;


            foreach (var schedule in gridSchedules)
            {
                if (previusApartment != schedule.ApartmentNumber)
                {
                    previusApartment = schedule.ApartmentNumber;
                    appointmentOrder++;
                    apartmentCounter = 0;
                }

                schedule.Encoding = $"{appointmentOrder}{apartmentCounter}{schedule.ApartmentNumber}-{towerId}";
                apartmentCounter++;
            }

            return gridSchedules;
        }

        public async Task<IEnumerable<ScheduleOrderDTO>> GetAllByUserAsync(int userId)
        {
            var scheduleOrders = await _scheduleOrderRepository.GetByUserAsync(userId);
            return scheduleOrders.Where(order => order.Status == ScheduleStatus.Opened).Select(order => (ScheduleOrderDTO)order).ToList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _scheduleOrderRepository.UpdateScheduleOrderAsync(id, ScheduleStatus.Canceled);
        }

        public async Task<bool> CreateAsync(ScheduleOrderDTO newSchedule)
        {
            return await _scheduleOrderRepository.CreateScheduleOrderAsync(newSchedule);
        }

        public async Task<bool> UpdateAsync(ScheduleOrderDTO newSchedule)
        {
            return await _scheduleOrderRepository.UpdateScheduleOrderAsync((ScheduleOrder)newSchedule);
        }
    }
}

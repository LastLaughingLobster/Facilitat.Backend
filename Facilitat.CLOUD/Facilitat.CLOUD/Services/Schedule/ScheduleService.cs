using Facilitat.CLOUD.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Facilitat.CLOUD.Repositories.Schedule;
using System.Linq;
using Facilitat.CLOUD.Models.Enums;
using Facilitat.CLOUD.Models.Entities;
using Facilitat.CLOUD.Models.Settings;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;

namespace Facilitat.CLOUD.Services.Schedule
{
    public class ScheduleOrderService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleOrderRepository;
        private readonly IModel _channel; // This should come from RabbitMQ.Client
        private readonly string _queueName; // Define the queue name

        public ScheduleOrderService(IScheduleRepository scheduleOrderRepository, IModel channel, MyRabbitMQSettings settings)
        {
            _scheduleOrderRepository = scheduleOrderRepository;
            _channel = channel;
            _queueName = settings.QueueName; // Assign the queue name from settings
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
            // Attempt to create the schedule order
            var createResult = await _scheduleOrderRepository.CreateScheduleOrderAsync(newSchedule);

            // Only proceed if the creation was successful
            if (createResult)
            {
                // Serialize the newSchedule object to a JSON string
                var messageBody = JsonConvert.SerializeObject(newSchedule);

                // Convert the JSON string to a byte array
                var messageBodyBytes = Encoding.UTF8.GetBytes(messageBody);

                // Publish the message to the queue
                _channel.BasicPublish(
                    exchange: "", // Use the default exchange
                    routingKey: _queueName, // Use the queue name as the routing key
                    basicProperties: null, // No message properties
                    body: messageBodyBytes // The message body
                );
            }

            // Return the result of the schedule creation
            return createResult;
        }

        public async Task<bool> UpdateAsync(ScheduleOrderDTO newSchedule)
        {
            return await _scheduleOrderRepository.UpdateScheduleOrderAsync((ScheduleOrder)newSchedule);
        }
    }
}

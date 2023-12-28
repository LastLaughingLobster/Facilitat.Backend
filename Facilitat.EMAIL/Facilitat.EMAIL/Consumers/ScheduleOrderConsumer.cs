using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Facilitat.EMAIL.Models.DTOs;
using Facilitat.EMAIL.Services.Schedule;

namespace Facilitat.EMAIL.Services
{
    public class ScheduleOrderConsumer : BackgroundService
    {
        private readonly IModel _channel;
        private readonly string _queueName;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ScheduleOrderConsumer(IModel channel, string queueName, IServiceScopeFactory serviceScopeFactory)
        {
            _channel = channel;
            _queueName = queueName;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var scheduleOrderDto = JsonConvert.DeserializeObject<ScheduleOrderDTO>(content);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scheduleService = scope.ServiceProvider.GetRequiredService<IScheduleService>();
                    await scheduleService.ProcessScheduleOrderAsync(scheduleOrderDto);
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_queueName, false, consumer);

            await Task.CompletedTask;
        }
    }
}

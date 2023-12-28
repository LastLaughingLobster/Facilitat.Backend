using Facilitat.EMAIL.Models.DTOs;
using Facilitat.EMAIL.Repositories.Schedule;
using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Facilitat.EMAIL.Models.Templates;

namespace Facilitat.EMAIL.Services.Schedule
{
    public class ScheduleOrderService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IAmazonSimpleEmailService _sesClient;
        private readonly IConfiguration _configuration;

        public ScheduleOrderService(
            IScheduleRepository scheduleRepository,
            IAmazonSimpleEmailService sesClient,
            IConfiguration configuration)
        {
            _scheduleRepository = scheduleRepository;
            _sesClient = sesClient;
            _configuration = configuration;
        }

        public async Task ProcessScheduleOrderAsync(ScheduleOrderDTO scheduleOrderDto)
        {
            var scheduleEmailDetails = await _scheduleRepository.GetScheduleEmailDetailsAsync(scheduleOrderDto);
            if (scheduleEmailDetails != null)
            {
                string emailContent = InterpolateEmailTemplate(
                    scheduleEmailDetails.Username,
                    scheduleEmailDetails.Tower,
                    scheduleEmailDetails.Apartment,
                    scheduleEmailDetails.Date);

                await SendEmailAsync(emailContent, scheduleEmailDetails.Email);
            }
        }

        private string InterpolateEmailTemplate(string username, string tower, string apartment, DateTime date)
        {
            string template = EmailTemplates.ScheduleEmailTemplate;
            template = template.Replace("{{UserName}}", username);
            template = template.Replace("{{Tower}}", tower);
            template = template.Replace("{{Apartment}}", apartment);
            template = template.Replace("{{Date}}", date.ToString("yyyy-MM-dd HH:mm"));

            return template;
        }


        private async Task SendEmailAsync(string emailContent, string recipientEmail)
        {
            var sendRequest = new SendEmailRequest
            {
                Source = _configuration["AWS:SESFromEmail"],
                Destination = new Destination { ToAddresses = new List<string> { recipientEmail } },
                Message = new Message
                {
                    Subject = new Content("Your Visit Schedule"),
                    Body = new Body { Html = new Content(emailContent) }
                }
            };

            await _sesClient.SendEmailAsync(sendRequest);
        }
    }
}

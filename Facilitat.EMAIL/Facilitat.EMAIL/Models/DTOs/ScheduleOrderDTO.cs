using Facilitat.EMAIL.Models.Entities;
using Facilitat.EMAIL.Models.Enums;
using System;

namespace Facilitat.EMAIL.Models.DTOs
{
    public class ScheduleOrderDTO
    {
        public int Id { get; set; }
        public int ApartmentId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Description { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public bool IsAllDay { get; set; }
        public ScheduleStatus Status { get; set; }

        public static implicit operator ScheduleOrderDTO(ScheduleOrder scheduleOrder)
        {
            if (scheduleOrder == null) return null;

            return new ScheduleOrderDTO
            {
                Id = scheduleOrder.Id,
                ApartmentId = scheduleOrder.ApartmentID,
                UserId = scheduleOrder.UserID,
                Title = scheduleOrder.Title,
                Start = scheduleOrder.ScheduledTime,
                End = scheduleOrder.EndTime,
                Description = scheduleOrder.Description,
                RecurrenceRule = scheduleOrder.RecurrenceRule,
                RecurrenceException = scheduleOrder.RecurrenceException,
                IsAllDay = scheduleOrder.IsAllDay ?? false,
                Status = scheduleOrder.Status
            };
        }

    }

}

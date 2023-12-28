using System;

namespace Facilitat.EMAIL.Models.DTOs
{
    public class ScheduleEmailDTO
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Apartment { get; set; }
        public string Tower { get; set; }
        public DateTime Date { get; set; }
    }
}

using Facilitat.EMAIL.Models.Entities;
using Facilitat.EMAIL.Models.Enums;
using System;

namespace Facilitat.EMAIL.Models.DTOs
{
    public class GridScheduleDTO
    {
        public int ScheduleId { get; set; }
        public int ApartmentNumber { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public  string Time { get; set; }
        public string Description { get; set; }
        public ScheduleStatus Status { get; set; }
        public string Encoding { get; set; }
    }

}

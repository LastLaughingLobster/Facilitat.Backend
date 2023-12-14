﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Facilitat.CLOUD.Models.Entities
{
    [Table("ScheduleOrder")]
    public class ScheduleOrder
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Apartment")]
        public int ApartmentID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public DateTime ScheduledTime { get; set; }

        public DateTime? EndTime { get; set; }

        [MaxLength(255)]
        public string ContractorName { get; set; }

        public string Purpose { get; set; }

        public string RecurrenceRule { get; set; }

        public string RecurrenceException { get; set; }

        public bool? IsAllDay { get; set; }

        public string Description { get; set; }

        public Apartment Apartment { get; set; }
        public User User { get; set; }
    }
}

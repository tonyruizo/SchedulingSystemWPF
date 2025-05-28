using System;
using System.ComponentModel.DataAnnotations;

namespace SchedulingSystemWPF.Models
{
    public class Appointment : ScheduleLoggin
    {
        public int AppointmentId { get; set; }

        public int CustomerId { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [StringLength(255)]
        public string Location { get; set; }

        [Required]
        [StringLength(255)]
        public string Contact { get; set; }

        [Required]
        [StringLength(255)]
        public string Type { get; set; }

        [Required]
        [StringLength(255)]
        public string Url { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}

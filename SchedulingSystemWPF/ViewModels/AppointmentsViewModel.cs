using System;

namespace SchedulingSystemWPF.ViewModels
{
    public class AppointmentsViewModel
    {
        public int AppointmentId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Contact { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }

        public DateTime StartLocal => TimeZoneInfo.ConvertTimeFromUtc(Start, TimeZoneInfo.Local);
        public DateTime EndLocal => TimeZoneInfo.ConvertTimeFromUtc(End, TimeZoneInfo.Local);
    }
}

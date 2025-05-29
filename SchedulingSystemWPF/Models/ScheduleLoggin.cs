using System;

namespace SchedulingSystemWPF.Models
{
    /// <summary>
    /// Models who and when it was created or updated.
    /// </summary>
    public abstract class ScheduleLoggin
    {
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; }
    }
}

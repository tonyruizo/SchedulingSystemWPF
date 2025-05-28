using System;
using System.ComponentModel.DataAnnotations;

namespace SchedulingSystemWPF.Models
{
    /// <summary>
    /// Models who and when it was created or updated.
    /// </summary>
    public abstract class ScheduleLoggin
    {
        public DateTime CreateDate { get; set; }

        [Required]
        [StringLength(40)]
        public string CreatedBy { get; set; }

        public DateTime LastUpdate { get; set; }

        [Required]
        [StringLength(40)]
        public string LastUpdateBy { get; set; }
    }
}

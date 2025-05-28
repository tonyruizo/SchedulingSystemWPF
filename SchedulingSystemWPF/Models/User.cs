using System.ComponentModel.DataAnnotations;

namespace SchedulingSystemWPF.Models
{
    public class User : ScheduleLoggin
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SchedulingSystemWPF.Models
{
    public class Customer : ScheduleLoggin
    {
        public int CustomerId { get; set; }

        [Required]
        [StringLength(45)]
        public string CustomerName { get; set; }

        public int AddressId { get; set; }

        public bool Active { get; set; }

    }
}

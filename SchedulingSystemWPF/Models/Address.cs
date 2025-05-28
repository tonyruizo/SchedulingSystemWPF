using System.ComponentModel.DataAnnotations;

namespace SchedulingSystemWPF.Models
{
    public class Address : ScheduleLoggin
    {
        public int AdressId { get; set; }

        [Required]
        [StringLength(50)]
        public string AddressName { get; set; }

        [Required]
        [StringLength(50)]
        public string AddressNameTwo { get; set; }

        public int CityId { get; set; }

        [Required]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }
    }
}

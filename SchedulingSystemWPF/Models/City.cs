using System.ComponentModel.DataAnnotations;

namespace SchedulingSystemWPF.Models
{
    public class City : ScheduleLoggin
    {
        public int CityId { get; set; }

        [Required]
        [StringLength(50)]
        public string CityName { get; set; }

        public int CountryId { get; set; }
    }
}

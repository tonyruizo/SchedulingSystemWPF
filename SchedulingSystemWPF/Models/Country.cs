using System.ComponentModel.DataAnnotations;

namespace SchedulingSystemWPF.Models
{
    /// <summary>
    /// A Customer's country.
    /// </summary>
    public class Country : ScheduleLoggin
    {
        public int CountryId { get; set; }

        [Required]
        [StringLength(50)]
        public string CountryName { get; set; }

    }
}

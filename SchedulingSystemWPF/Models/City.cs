namespace SchedulingSystemWPF.Models
{
    public class City : ScheduleLoggin
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}

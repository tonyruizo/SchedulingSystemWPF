namespace SchedulingSystemWPF.Models
{
    public class Address : ScheduleLoggin
    {
        public int AdressId { get; set; }
        public string AddressName { get; set; }
        public string AddressNameTwo { get; set; }
        public int CityId { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
    }
}

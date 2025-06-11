namespace SchedulingSystemWPF.Models
{
    public class Address : ScheduleLoggin
    {
        public int AdressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }

    }
}

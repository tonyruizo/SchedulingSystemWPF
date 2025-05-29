namespace SchedulingSystemWPF.Models
{
    public class User : ScheduleLoggin
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

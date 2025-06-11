using SchedulingSystemWPF.Models;

namespace SchedulingSystemWPF.Services
{
    public static class SessionManager
    {
        public static User LoggedInUser { get; set; }
        public static int UserId => LoggedInUser?.UserId ?? 0;
        public static string Username => LoggedInUser.UserName ?? "N/A";
    }
}

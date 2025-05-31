
using MySql.Data.MySqlClient;
using System.Configuration;

namespace SchedulingSystemWPF.DatabaseAccess
{
    /// <summary>
    /// Connects to the database.
    /// </summary>
    public static class DbConnect
    {
        public static MySqlConnection GetConnection()
        {
            string constr = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

            return new MySqlConnection(constr);
        }
    }
}

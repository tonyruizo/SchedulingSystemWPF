
using MySql.Data.MySqlClient;
using System.Configuration;

namespace SchedulingSystemWPF.DatabaseAccess
{
    /// <summary>
    /// Provides functionality for creating MySQL database connections using configuration settings.
    /// </summary>
    public static class DbConnect
    {
        /// <summary>
        /// Creates and returns a new MySQL database connection using the connection string  specified in the
        /// application's configuration file.
        /// </summary>
        /// <returns>A new instance of <see cref="MySqlConnection"/> initialized with the connection  string from the
        /// application's configuration file.</returns>
        public static MySqlConnection GetConnection()
        {
            string constr = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;

            return new MySqlConnection(constr);
        }
    }
}

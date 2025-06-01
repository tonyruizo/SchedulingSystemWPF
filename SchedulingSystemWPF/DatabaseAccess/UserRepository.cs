using MySql.Data.MySqlClient;

namespace SchedulingSystemWPF.DatabaseAccess
{
    /// <summary>
    /// A repository handles all operations related to the 'User' table in the MySQL database.
    /// </summary>
    public class UserRepository
    {
        /// <summary>
        /// Validate and check if user exist in the database.
        /// </summary>
        /// <param name="username">The login username</param>
        /// <param name="password">The login password</param>
        /// <returns>True or False</returns>
        public bool ValidateUser(string username, string password)
        {
            try
            {
                using (var conn = DbConnect.GetConnection())
                {
                    conn.Open();

                    // SQL Query
                    string cmdQuery = "SELECT COUNT(*) FROM user WHERE userName = @username AND password = @password";

                    using (var cmd = new MySqlCommand(cmdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        // Check if record exists
                        long count = (long)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }
    }
}

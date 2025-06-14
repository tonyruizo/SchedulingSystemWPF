using MySql.Data.MySqlClient;
using SchedulingSystemWPF.Models;
using System;
using System.Windows;

namespace SchedulingSystemWPF.DatabaseAccess
{
    /// <summary>
    /// A repository handles all operations related to the 'user' table in the MySQL database.
    /// </summary>
    public class UserRepository
    {
        /// <summary>
        /// Validate and retrieve user from the database.
        /// </summary>
        /// <param name="username">The login username</param>
        /// <param name="password">The login password</param>
        /// <returns>A user or null</returns>
        public User ValidateUser(string username, string password)
        {
            try
            {
                using (var conn = DbConnect.GetConnection())
                {
                    conn.Open();

                    // SQL Query
                    string cmdQuery = "SELECT * FROM user WHERE userName = @username AND password = @password LIMIT 1";

                    using (var cmd = new MySqlCommand(cmdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                User user = new User
                                {
                                    UserId = Convert.ToInt32(reader["userId"]),
                                    UserName = reader["userName"].ToString()
                                };
                                return user;
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Sorry! Internal error, please try again later.");
            }

            return null;
        }
    }
}

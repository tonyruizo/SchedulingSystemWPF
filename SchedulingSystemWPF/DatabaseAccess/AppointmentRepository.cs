using MySql.Data.MySqlClient;
using SchedulingSystemWPF.Models;
using System.Collections.Generic;

namespace SchedulingSystemWPF.DatabaseAccess
{
    /// <summary>
    /// A repository handles all operations related to the 'appointment' table in the MySQL database.
    /// </summary>
    public class AppointmentRepository
    {
        /// <summary>
        /// Get all appointments from the Appointment table.
        /// </summary>
        /// <returns>A list of appointments.</returns>
        public List<Appointment> GetAllAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();

            try
            {
                using (var conn = DbConnect.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM appointment";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            appointments.Add(new Appointment
                            {
                                AppointmentId = reader.GetInt32("appointmentId"),
                                CustomerId = reader.GetInt32("customerId"),
                                UserId = reader.GetInt32("userId"),
                                Title = reader.GetString("title"),
                                Description = reader.GetString("description"),
                                Location = reader.GetString("location"),
                                Contact = reader.GetString("contact"),
                                Type = reader.GetString("type"),
                                Url = reader.GetString("url"),
                                Start = reader.GetDateTime("start"),
                                End = reader.GetDateTime("end"),
                                CreateDate = reader.GetDateTime("createDate"),
                                CreatedBy = reader.GetString("createdBy"),
                                LastUpdate = reader.GetDateTime("lastUpdate"),
                                LastUpdateBy = reader.GetString("lastUpdateBy")
                            });
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }

            return appointments;
        }

        /// <summary>
        /// Add a new appointment and add it to the database.
        /// </summary>
        //    public void AddAppointment()
        //    {
        //        try
        //        {

        //        }
        //        catch (MySqlException ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //public void EditAppointment()
        //{
        //    try
        //    {

        //    }
        //    catch (MySqlException ex)
        //    {
        //        throw ex;
        //    }
        //}


        //public void DeleteAppointment()
        //{
        //    try
        //    {

        //    }
        //    catch (MySqlException ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}

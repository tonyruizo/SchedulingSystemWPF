using MySql.Data.MySqlClient;
using SchedulingSystemWPF.Models;
using SchedulingSystemWPF.ViewModels;
using System.Collections.Generic;

namespace SchedulingSystemWPF.DatabaseAccess
{
    /// <summary>
    /// A repository handles all operations related to the 'appointment' table in the MySQL database.
    /// </summary>
    public class AppointmentRepository
    {
        /// <summary>
        /// Get all appointments.
        /// </summary>
        /// <returns>A list of appointments.</returns>
        public List<AppointmentsViewModel> GetAllAppointments()
        {
            List<AppointmentsViewModel> appointments = new List<AppointmentsViewModel>();

            try
            {
                using (var conn = DbConnect.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT a.*, c.customerName
                        FROM appointment a
                        JOIN customer c ON a.customerId = c.customerId";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            appointments.Add(new AppointmentsViewModel
                            {
                                AppointmentId = reader.GetInt32("appointmentId"),
                                CustomerId = reader.GetInt32("customerId"),
                                CustomerName = reader.GetString("customerName"),
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
        public void AddAppointment(Appointment appointment, string createdBy)
        {
            try
            {

            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

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

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
        /// Add a new appointment to the database.
        /// </summary>
        /// <param name="appointment">The appointment object to add.</param>
        /// <param name="createdBy">The username of the user creating the appointment.</param>
        public void AddAppointment(Appointment appointment, string createdBy)
        {
            try
            {
                using (var conn = DbConnect.GetConnection())
                {
                    conn.Open();

                    string cmdQuery = @"
                        INSERT INTO appointment(customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy)
                        VALUE(@customerId, @userId, @title, @description,@location, @contact, @type, @url, @start, @end, NOW(), @createdBy, NOW(), @createdBy);";

                    using (var cmd = new MySqlCommand(cmdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", appointment.CustomerId);
                        cmd.Parameters.AddWithValue("@userId", appointment.UserId);
                        cmd.Parameters.AddWithValue("@title", appointment.Title);
                        cmd.Parameters.AddWithValue("@description", appointment.Description);
                        cmd.Parameters.AddWithValue("@location", appointment.Location);
                        cmd.Parameters.AddWithValue("@contact", appointment.Contact);
                        cmd.Parameters.AddWithValue("@type", appointment.Type);
                        cmd.Parameters.AddWithValue("@url", appointment.Url);
                        cmd.Parameters.AddWithValue("@start", appointment.Start);
                        cmd.Parameters.AddWithValue("@end", appointment.End);
                        cmd.Parameters.AddWithValue("@createdby", createdBy);

                        cmd.ExecuteNonQuery();
                    }

                }
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


        public void DeleteAppointment(int appointmentId)
        {
            try
            {
                using (var conn = DbConnect.GetConnection())
                {
                    conn.Open();

                    string cmdQuery = @"DELETE FROM appointment WHERE appointmentId = @appointmentId";

                    using (var cmd = new MySqlCommand(cmdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@appointmentId", appointmentId);

                        cmd.ExecuteNonQuery();
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

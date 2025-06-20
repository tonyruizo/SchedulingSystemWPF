using MySql.Data.MySqlClient;
using SchedulingSystemWPF.Models;
using SchedulingSystemWPF.ViewModels;
using System;
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
                throw new Exception($"Failed to retrieve appointments: {ex.Message}", ex);
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
                        VALUES (@customerId, @userId, @title, @description,@location, @contact, @type, @url, @start, @end, NOW(), @createdBy, NOW(), @createdBy);";

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
                        cmd.Parameters.AddWithValue("@start", appointment.Start.ToUniversalTime());
                        cmd.Parameters.AddWithValue("@end", appointment.End.ToUniversalTime());
                        cmd.Parameters.AddWithValue("@createdBy", createdBy);

                        int rowsAfected = cmd.ExecuteNonQuery();

                        // Needs 1 row affected to ensure a new record added
                        if (rowsAfected != 1)
                        {
                            throw new Exception("Failed to add appointment: No rows were inserted.");
                        }
                    }

                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Failed to add appointment to database: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates the details of an existing appointment in the database.
        /// </summary>
        /// <remarks>This method updates the appointment record in the database with the values provided
        /// in the <paramref name="appointment"/> object.</remarks>
        /// <param name="appointment">The <see cref="Appointment"/> object containing the updated appointment details.</param>
        /// <param name="updatedBy">The username or identifier of the user making the update.</param>
        public void EditAppointment(Appointment appointment, string updatedBy)
        {
            try
            {
                using (var conn = DbConnect.GetConnection())
                {
                    conn.Open();

                    string cmdQuery = @"
                        UPDATE appointment
                        SET
                            customerId = @customerId,
                            userId = @userId,
                            title = @title,
                            description = @description,
                            location = @location,
                            contact = @contact,
                            type = @type,
                            url = @url,
                            start = @start,
                            end = @end,
                            lastUpdate = NOW(),
                            lastUpdateBy = @lastUpdateBy
                       WHERE appointmentId = @appointmentId";

                    using (var cmd = new MySqlCommand(cmdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@appointmentId", appointment.AppointmentId);
                        cmd.Parameters.AddWithValue("@customerId", appointment.CustomerId);
                        cmd.Parameters.AddWithValue("@userId", appointment.UserId);
                        cmd.Parameters.AddWithValue("@title", appointment.Title);
                        cmd.Parameters.AddWithValue("@description", appointment.Description);
                        cmd.Parameters.AddWithValue("@location", appointment.Location);
                        cmd.Parameters.AddWithValue("@contact", appointment.Contact);
                        cmd.Parameters.AddWithValue("@type", appointment.Type);
                        cmd.Parameters.AddWithValue("@url", appointment.Url);
                        cmd.Parameters.AddWithValue("@start", appointment.Start.ToUniversalTime());
                        cmd.Parameters.AddWithValue("@end", appointment.End.ToUniversalTime());
                        cmd.Parameters.AddWithValue("@lastUpdate", DateTime.UtcNow);
                        cmd.Parameters.AddWithValue("@lastUpdateBy", updatedBy);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new Exception($"No appointment found with ID {appointment.AppointmentId}.");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Failed to update appointment: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes an appointment from the database based on the specified appointment ID.
        /// </summary>
        /// <remarks>
        /// This method removes the appointment record with the specified ID from the database.
        /// </remarks>
        /// <param name="appointmentId">The unique ID of the appointment to be deleted.</param>
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

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new Exception($"No appointment found with ID {appointmentId}.");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Failed to delete appointment: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get the the upcoming appointments withing a time range.
        /// </summary>
        /// <param name="userId">The ID of the current user in session.</param>
        /// <param name="start">The start time (Now).</param>
        /// <param name="end">The end of the time range (e.g., 15 minutes from now)</param>
        /// <returns>A List of upcoming appointments.</returns>
        public List<AppointmentsViewModel> GetUpcomingAppointments(int userId, DateTime start, DateTime end)
        {
            List<AppointmentsViewModel> appointments = new List<AppointmentsViewModel>();

            try
            {
                using (var conn = DbConnect.GetConnection())
                {
                    conn.Open();

                    string cmdQuery = @"
                        SELECT a.*, c.customerName
                        FROM appointment a
                        JOIN customer c ON a.customerId = c.customerId
                        WHERE a.userId = @userId
                        AND a.start >= @start
                        AND a.start <= @end";

                    using (var cmd = new MySqlCommand(cmdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@start", start);
                        cmd.Parameters.AddWithValue("@end", end);

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
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve upcoming appointmets: {ex.Message}", ex);
            }

            return appointments;
        }
    }
}

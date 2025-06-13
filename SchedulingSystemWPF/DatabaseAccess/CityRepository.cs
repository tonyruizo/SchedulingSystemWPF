using MySql.Data.MySqlClient;
using System;

namespace SchedulingSystemWPF.DatabaseAccess
{
    /// <summary>
    /// Handles all operations related to 'city' table in MySQL database.
    /// </summary>
    public class CityRepository
    {
        /// <summary>
        /// Retrieves the unique identifier for a city by its name and country, or inserts a new city if it does not
        /// exist.
        /// </summary>
        /// <remarks>This method either retrieves the ID of an existing city or inserts a new city into
        /// the database. If the city is inserted, the <paramref name="createdBy"/> parameter is used to track the
        /// creator.</remarks>
        /// <param name="cityName">The name of the city to retrieve or insert. Cannot be null or empty.</param>
        /// <param name="countryId">The unique identifier of the country to which the city belongs. Must be a valid country ID.</param>
        /// <param name="createdBy">The username or identifier of the user creating the city.</param>
        /// <returns>The unique identifier of the city. Returns -1 if the operation fails.</returns>
        public int GetOrInsertCity(string cityName, int countryId, string createdBy)
        {
            // Invalid ID / Negative value
            int cityId = -1;

            using (var conn = DbConnect.GetConnection())
            {
                conn.Open();

                var selectCmd = new MySqlCommand("SELECT cityId FROM city WHERE city = @city AND countryId = @countryId;", conn);

                selectCmd.Parameters.AddWithValue("@city", cityName);
                selectCmd.Parameters.AddWithValue("@countryId", countryId);

                // Return a matching city or null
                var result = selectCmd.ExecuteScalar();

                if (result != null)
                {
                    // If found matching city id, Get the city id
                    return Convert.ToInt32(result);
                }

                // Insert and retrive
                var insertCmd = new MySqlCommand(
                    @"INSERT INTO city (city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy)
                  VALUES (@city, @countryId, NOW(), @createdBy, NOW(), @createdBy);
                  SELECT LAST_INSERT_ID();", conn
                  );

                insertCmd.Parameters.AddWithValue("@city", cityName);
                insertCmd.Parameters.AddWithValue("@countryId", countryId);
                insertCmd.Parameters.AddWithValue("@createdBy", createdBy);

                // Excecute and reassign cityId
                cityId = Convert.ToInt32(insertCmd.ExecuteScalar());
            }

            return cityId;
        }
    }
}

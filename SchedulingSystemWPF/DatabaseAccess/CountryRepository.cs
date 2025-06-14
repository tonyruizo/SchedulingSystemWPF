using MySql.Data.MySqlClient;
using System;

namespace SchedulingSystemWPF.DatabaseAccess
{
    /// <summary>
    /// Handles all operations related to 'country' table in MySQL database.
    /// </summary>
    public class CountryRepository
    {
        /// <summary>
        /// Retrieves the identifier of the specified country, or inserts a new country if it does not exist.
        /// </summary>
        /// <remarks>If the country already exists, its identifier is returned. If the country does not
        /// exist, it is added to the database and its new identifier is returned. Ensure that <paramref
        /// name="countryName"/> is unique to avoid unintended behavior.</remarks>
        /// <param name="countryName">The name of the country to retrieve or insert.</param>
        /// <param name="createdBy">The name of the user creating the country.</param>
        /// <returns>The unique identifier of the country. Returns a negative value if the operation fails.</returns>
        public int GetOrInsertCountry(string countryName, string createdBy)
        {
            // Invalid ID / Negative value
            int countryId = -1;

            using (var conn = DbConnect.GetConnection())
            {
                conn.Open();

                var selectCmd = new MySqlCommand("SELECT countryId FROM country WHERE country = @country;", conn);

                selectCmd.Parameters.AddWithValue("@country", countryName);

                // Return a matching country or null
                var result = selectCmd.ExecuteScalar();

                if (result != null)
                {
                    // If found matching country id, Get the country id
                    return Convert.ToInt32(result);
                }

                // Insert new country and retrive Id
                var insertCmd = new MySqlCommand(
                    @"INSERT INTO country (country, createDate, createdBy, lastUpdate, lastUpdateBy) 
                    VALUES (@country, NOW(), @createdBy, NOW(), @createdBy);
                    SELECT LAST_INSERT_ID();", conn
                );

                insertCmd.Parameters.AddWithValue("@country", countryName);
                insertCmd.Parameters.AddWithValue("@createdBy", createdBy);

                // Execute and reassign it to countryId
                countryId = Convert.ToInt32(insertCmd.ExecuteScalar());
            }

            return countryId;
        }
    }
}

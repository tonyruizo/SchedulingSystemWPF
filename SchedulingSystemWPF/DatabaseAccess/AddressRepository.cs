using MySql.Data.MySqlClient;
using SchedulingSystemWPF.Services;
using System;

namespace SchedulingSystemWPF.DatabaseAccess
{
    /// <summary>
    /// Handles all operations related to 'address' table in MySQL database. 
    /// </summary>
    public class AddressRepository
    {
        public int GetOrInsertAddress(string addressLine1, string addressLine2, string postalCode, string phone, int cityId, string createdBy)
        {
            int addressId = -1;

            using (var conn = DbConnect.GetConnection())
            {
                conn.Open();

                var selectCmd = new MySqlCommand(
                    @"SELECT addressId FROM address 
                    WHERE address = @address AND address2 = @address2 
                    AND postalCode = @postalCode AND phone = @phone 
                    AND cityId = @cityId", conn
                );

                selectCmd.Parameters.AddWithValue("@address", addressLine1);
                selectCmd.Parameters.AddWithValue("@address2", addressLine2);
                selectCmd.Parameters.AddWithValue("@postalCode", postalCode);
                selectCmd.Parameters.AddWithValue("@phone", phone);
                selectCmd.Parameters.AddWithValue("@cityId", cityId);


                var result = selectCmd.ExecuteScalar();

                if (result != null)
                {
                    return Convert.ToInt32(result);
                }

                // If not found, insert it
                var insertCmd = new MySqlCommand(
                    @"INSERT INTO address (address, address2, postalCode, phone, cityId, createDate, createdBy, lastUpdate, lastUpdateBy)
                    VALUES(@address, @address2, @postalCode, @phone, @cityId, NOW(), @createdBy, NOW(), @createdBy);
                    SELECT LAST_INSERT_ID();", conn
                );

                insertCmd.Parameters.AddWithValue("@address", addressLine1);
                insertCmd.Parameters.AddWithValue("@address2", addressLine2);
                insertCmd.Parameters.AddWithValue("@postalCode", postalCode);
                insertCmd.Parameters.AddWithValue("@phone", phone);
                insertCmd.Parameters.AddWithValue("@cityId", cityId);
                insertCmd.Parameters.AddWithValue("@createdBy", SessionManager.Username);

                addressId = Convert.ToInt32(insertCmd.ExecuteScalar());
            }

            return addressId;

        }

        /// <summary>
        /// Updates an existing address in the database.
        /// </summary>
        /// <param name="addressId">The ID of the address to update.</param>
        /// <param name="address">The updated street address (line 1).</param>
        /// <param name="address2">The updated address line 2 (optional).</param>
        /// <param name="postalCode">The updated postal code.</param>
        /// <param name="phone">The updated phone number.</param>
        /// <param name="cityId">The updated city ID.</param>
        /// <param name="updatedBy">The username of the user updating the address.</param>
        public void UpdateAddress(int addressId, string address, string address2, string postalCode, string phone, int cityId, string updatedBy)
        {
            try
            {
                using (var conn = DbConnect.GetConnection())
                {
                    conn.Open();

                    string cmdQuery = @"
                UPDATE address
                SET 
                    address = @address,
                    address2 = @address2,
                    postalCode = @postalCode,
                    phone = @phone,
                    cityId = @cityId,
                    lastUpdate = NOW(),
                    lastUpdateBy = @updatedBy
                WHERE addressId = @addressId";

                    using (var cmd = new MySqlCommand(cmdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@addressId", addressId);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@address2", address2 ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@postalCode", postalCode);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@cityId", cityId);
                        cmd.Parameters.AddWithValue("@updatedBy", updatedBy);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new Exception($"No address found with ID {addressId}.");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Failed to update address: {ex.Message}", ex);
            }
        }
    }
}

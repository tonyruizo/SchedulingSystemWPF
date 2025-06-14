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
    }
}

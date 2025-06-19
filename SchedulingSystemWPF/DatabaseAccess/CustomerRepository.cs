using MySql.Data.MySqlClient;
using SchedulingSystemWPF.Models;
using System;
using System.Collections.Generic;

namespace SchedulingSystemWPF.DatabaseAccess
{
    /// <summary>
    /// Handles all operations related to 'customer' table in MySQL database.
    /// </summary>
    public class CustomerRepository
    {
        /// <summary>
        /// Get all customers.
        /// </summary>
        /// <returns>A list of customers.</returns>
        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            try
            {
                using (var conn = DbConnect.GetConnection())
                {
                    conn.Open();

                    string cmdQuery = @"
                        SELECT 
                            c.customerId,
                            c.customerName,
                            c.addressId,
                            c.active,
                            c.createDate,
                            c.createdBy,
                            c.lastUpdate,
                            c.lastUpdateBy,

                            a.address AS AddressLine1,
                            a.address2 AS AddressLine2,
                            a.postalCode,
                            a.phone,

                            ci.city,
                            co.country

                        FROM customer c
                        JOIN address a ON c.addressId = a.addressId
                        JOIN city ci ON a.cityId = ci.cityId
                        JOIN country co ON ci.countryId = co.countryId";

                    using (var cmd = new MySqlCommand(cmdQuery, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                CustomerId = reader.GetInt32("customerId"),
                                CustomerName = reader.GetString("customerName"),
                                AddressId = reader.GetInt32("addressId"),
                                AddressLine1 = reader.GetString("AddressLine1"),
                                AddressLine2 = reader.GetString("AddressLine2"),
                                PostalCode = reader.GetString("postalCode"),
                                Phone = reader.GetString("phone"),
                                City = reader.GetString("city"),
                                Country = reader.GetString("country"),
                                Active = reader.GetBoolean("Active"),
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
                throw new Exception($"Failed to retrieve customers: {ex.Message}", ex);
            }

            return customers;
        }

        /// <summary>
        /// Add new customer data to the database.
        /// </summary>
        /// <param name="nameInput">The customer name to add.</param>
        /// <param name="addressId">The address Id associated with the customer.</param>
        /// <param name="createdBy">The username of the user creating the customer form.</param>
        public void AddCustomer(string nameInput, int addressId, string createdBy)
        {
            try
            {
                using (var conn = DbConnect.GetConnection())
                {
                    conn.Open();

                    string cmdQuery = @"
                        INSERT INTO customer(customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy)
                        VALUES (@nameInput, @addressId, 1, NOW(), @createdBy, NOW(), @createdBy)";

                    using (var cmd = new MySqlCommand(cmdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@nameInput", nameInput);
                        cmd.Parameters.AddWithValue("@addressId", addressId);
                        cmd.Parameters.AddWithValue("@createdBy", createdBy);

                        int rowsAfected = cmd.ExecuteNonQuery();

                        // Needs 1 row affected to ensure a new record added
                        if (rowsAfected != 1)
                        {
                            throw new Exception("Failed to add a customer: No rows were inserted.");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Failed to add customer to database: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates the details of an existing customer in the database.
        /// </summary>
        /// <remarks>This method updates the customer record.</remarks>
        public void EditCustomer(int customerId, string customerName, int addressId, string updatedBy)
        {
            try
            {
                using (var conn = DbConnect.GetConnection())
                {
                    conn.Open();

                    string cmdQuery = @"
                        UPDATE customer
                        SET 
                            customerName = @customerName,
                            addressId = @addressId,
                            lastUpdate = NOW(),
                            lastUpdateBy = @updatedBy
                        WHERE customerId = @customerId;";

                    using (var cmd = new MySqlCommand(cmdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.Parameters.AddWithValue("@customerName", customerName);
                        cmd.Parameters.AddWithValue("@addressId", addressId);
                        cmd.Parameters.AddWithValue("@updatedBy", updatedBy);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new Exception($"No customer found with ID.");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Failed to update customer: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a customer from the database based on the specified customer ID.
        /// </summary>
        /// <remarks>
        /// This method removes the customer record with the specified ID from the database.
        /// </remarks>
        /// <param name="customerId">The unique ID of the customer to be deleted.</param>
        public void DeleteCustomer(int customerId)
        {
            try
            {
                using (var conn = DbConnect.GetConnection())
                {
                    conn.Open();

                    string cmdQuery = @"DELETE FROM customer WHERE customerId = @customerId";

                    using (var cmd = new MySqlCommand(cmdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("customerId", customerId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            throw new Exception($"No customer found with ID {customerId}.");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Failed to delete customer: {ex.Message}", ex);
            }
        }
    }
}

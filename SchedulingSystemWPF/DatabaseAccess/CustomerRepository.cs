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

        //public void EditCustomer()
        //{
        //    try
        //    {

        //    }
        //    catch (MySqlException ex)
        //    {
        //        throw ex;
        //    }
        //}


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

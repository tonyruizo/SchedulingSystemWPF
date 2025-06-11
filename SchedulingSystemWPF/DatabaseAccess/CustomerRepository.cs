using MySql.Data.MySqlClient;
using SchedulingSystemWPF.Models;
using System.Collections.Generic;
using System.Windows;

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
                                CustomerId = reader.GetInt32("customerid"),
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
                throw ex;
            }

            return customers;
        }

        /// <summary>
        /// Add a new customer and add it to the database.
        /// </summary>
        public void AddCustomer(string customerName, string customerAddress)
        {
            try
            {
                MessageBox.Show($"Added  {customerName}, and {customerAddress}");
            }
            catch (MySqlException ex)
            {
                throw ex;
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


        //public void DeleteCustomert()
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

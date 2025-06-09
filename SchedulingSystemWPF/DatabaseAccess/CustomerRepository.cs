using MySql.Data.MySqlClient;
using SchedulingSystemWPF.Models;
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

                    string cmdQuery = "SELECT * FROM customer;";

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
        //    public void AddCustomer()
        //    {
        //        try
        //        {

        //        }
        //        catch (MySqlException ex)
        //        {
        //            throw ex;
        //        }
        //    }

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

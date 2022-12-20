

using Metaforge_Marketing.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Metaforge_Marketing.Repository
{
    public class CustomersRepository
    {
        #region Insert Queries
        public static void InsertToDB(SqlConnection connection, Customer customer)
        {

        }
        #endregion Insert Queries


        #region Select Queries

        /// <summary>
        /// Returns the total count of customers in database
        /// Used in pagination
        /// </summary>
        /// <param name="connection"></param>
        /// <returns> int :=> count of the customers</returns>
        public static int CountCustomers(SqlConnection connection)
        {
            using(SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(Id) FROM Customers";
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        /// <summary>
        /// Returns a list of Customers in the database
        /// Does not fetch all rows, but takes input the Offset Index, and number of rows to fetch
        /// Used in pagination
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="offsetIndex"></param>
        /// <param name="entriesPerPage"></param>
        /// <returns></returns>
        public static IEnumerable<Customer> FetchCustomers(SqlConnection connection, int offsetIndex, int entriesPerPage)
        {
            List<Customer> customers = new List<Customer>();

            using(SqlCommand cmd = new SqlCommand("FetchCustomers", connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@offsetIndex", System.Data.SqlDbType.Int).Value = offsetIndex;
                cmd.Parameters.Add("@entriesPerPage", System.Data.SqlDbType.Int).Value =entriesPerPage;

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Customer customer = new Customer
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            CustomerName = reader["CustomerName"].ToString(),
                            City = reader["City"].ToString(),
                            Address = reader["Address"].ToString(),
                            Pincode = reader["Pincode"].ToString()
                        };
                        customers.Add(customer);
                    }
                    reader.Close();
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            return customers;
        }


        /// <summary>
        /// Based on a search text for Customer's Name, searches the database and returns the results
        /// Used mainly in pagination 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="customerName"></param>
        /// <returns></returns>
        public static IEnumerable<Customer> FetchCustomers(SqlConnection connection, string customerName)
        {
            List<Customer> customers = new List<Customer>();

            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Customers WHERE CustomerName LIKE @customerName";
                cmd.Parameters.Add("@customerName", System.Data.SqlDbType.VarChar).Value = $"%{customerName}%";

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Customer customer = new Customer
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        CustomerName = reader["CustomerName"].ToString(),
                        City = reader["City"].ToString(),
                        Address = reader["Address"].ToString(),
                        Pincode = reader["Pincode"].ToString()
                    };
                    customers.Add(customer);
                }
                reader.Close();
            }
            MessageBox.Show(customers.Count.ToString());
            return customers;
        }

        #endregion SelectQueries
    }

}

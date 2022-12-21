

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

        /// <summary>
        /// Given a customer, inserts it in the database, and if there are buyers too, inserts them in the database too
        /// Used when adding a new Customer to the database
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="customer"></param>
        public static void InsertToDB(SqlConnection connection, Customer customer)
        {
            using(SqlCommand cmd = connection.CreateCommand())
            {
                string BuyerInsertQuery = "INSERT INTO Buyers (BuyerName, Email, Phone, CustId) VALUES (@buyerName, @email, @phone, @custId)";
                string CustomerInsertQuery = "INSERT INTO Customers (CustomerName, City, Address, Pincode) VALUES(@customerName, @city, @address, @pincode)";

                cmd.Parameters.Add("@customerName", System.Data.SqlDbType.VarChar).Value = customer.CustomerName;
                cmd.Parameters.Add("@city", System.Data.SqlDbType.VarChar).Value = customer.City;
                cmd.Parameters.Add("@address", System.Data.SqlDbType.VarChar).Value = customer.Address;
                cmd.Parameters.Add("@pincode", System.Data.SqlDbType.VarChar).Value = customer.Pincode;
                try
                {
                    cmd.CommandText = CustomerInsertQuery;
                    int CustId = Convert.ToInt32(cmd.ExecuteScalar());
                    if (customer.Buyers.Count > 0)
                    {
                        foreach (Buyer buyer in customer.Buyers)
                        {
                            try
                            {
                                cmd.CommandText = BuyerInsertQuery;
                                cmd.Parameters.Add("@custId", System.Data.SqlDbType.Int).Value = CustId; 
                                cmd.Parameters.Add("@buyerName", System.Data.SqlDbType.VarChar).Value = buyer.Name;
                                cmd.Parameters.Add("@email", System.Data.SqlDbType.VarChar).Value = buyer.Email;
                                if (!String.IsNullOrEmpty(buyer.Phone))
                                {
                                    cmd.Parameters.Add("@phone", System.Data.SqlDbType.VarChar).Value = buyer.Phone;
                                }

                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear();
                            }
                            catch (Exception e2)
                            {
                                MessageBox.Show(e2.Message);
                            }
                        }
                    }
                }
                catch(Exception e1)
                {
                    MessageBox.Show(e1.Message);
                }
            }
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
        /// Given a customer, counts the number of buyers it has
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="customer"></param>
        /// <returns>Integer- number of buyers</returns>
        public static int CountBuyers(SqlConnection conn, Customer customer)
        {
            using(SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(Id) FROM Buyers WHERE CustId = @custId";
                cmd.Parameters.Add("@custId", System.Data.SqlDbType.Int).Value= customer.Id;
                return Convert.ToInt32(cmd.ExecuteScalar());
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

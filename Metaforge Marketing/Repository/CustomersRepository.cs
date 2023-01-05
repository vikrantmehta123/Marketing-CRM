

using Metaforge_Marketing.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

namespace Metaforge_Marketing.Repository
{
    public class CustomersRepository
    {
        #region Insert Queries

        // Summary:
        //      Inserts a Customer in the database
        //      Used in "Add Customer" form
        // Parameters:
        //      Customer- The customer which needs to be inserted
        //      buyers- A list of buyers which needs to be inserted along with the Customer
        //              If there are no buyers, pass empty list
        public static void InsertToDB(SqlConnection connection, Customer customer, IEnumerable<Buyer> buyers)
        {
            SqlTransaction transaction = connection.BeginTransaction();

            // Add the customer insert query
            SqlCommand CustomerInsertCommand = new SqlCommand("INSERT INTO Customers (CustomerName, City, Address, Pincode) VALUES(@customerName, @city, @address, @pincode) SELECT SCOPE_IDENTITY()", connection, transaction);
            CustomerInsertCommand.Parameters.Add("@customerName", System.Data.SqlDbType.VarChar).Value = customer.CustomerName;
            CustomerInsertCommand.Parameters.Add("@city", System.Data.SqlDbType.VarChar).Value = customer.City;
            CustomerInsertCommand.Parameters.Add("@address", System.Data.SqlDbType.VarChar).Value = customer.Address;
            CustomerInsertCommand.Parameters.Add("@pincode", System.Data.SqlDbType.VarChar).Value = customer.Pincode;

            SqlCommand BuyerInsertCommand = new SqlCommand("INSERT INTO Buyers (BuyerName, Email, Phone, CustId) VALUES (@buyerName, @email, @phone, @custId)", connection, transaction);
            try
            {
                int CustId = Convert.ToInt32(CustomerInsertCommand.ExecuteScalar());
                if (buyers.Count() > 0)
                {
                    // Insert buyers
                    foreach (Buyer buyer in buyers)
                    {
                        BuyerInsertCommand.Parameters.Add("@custId", System.Data.SqlDbType.Int).Value = CustId; 
                        BuyerInsertCommand.Parameters.Add("@buyerName", System.Data.SqlDbType.VarChar).Value = buyer.Name;
                        BuyerInsertCommand.Parameters.Add("@email", System.Data.SqlDbType.VarChar).Value = buyer.Email;
                        if (!String.IsNullOrEmpty(buyer.Phone)) { BuyerInsertCommand.Parameters.Add("@phone", System.Data.SqlDbType.VarChar).Value = buyer.Phone; }
                        else { BuyerInsertCommand.Parameters.Add("@phone", System.Data.SqlDbType.VarChar).Value = DBNull.Value; }

                        BuyerInsertCommand.ExecuteNonQuery();
                        BuyerInsertCommand.Parameters.Clear();
                    }
                }
                transaction.Commit();
                MessageBox.Show("Successfully inserted");
            }
            catch(Exception e1)
            {
                MessageBox.Show(e1.Message);
                try
                {
                    transaction.Rollback(); // Rollback if errors
                }
                catch(Exception e2)
                {
                    MessageBox.Show(e2.Message);
                }
            }
            finally { transaction.Dispose(); }
        }
        #endregion Insert Queries


        #region Select Queries
        // Summary:
        //      Returns the number of customers in database
        public static int CountCustomers(SqlConnection connection)
        {
            using(SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(Id) FROM Customers";
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        // Summary:
        //      Fetches a list of Customers for pagination
        public static IEnumerable<Customer> FetchCustomers(SqlConnection connection, int offsetIndex, int entriesPerPage)
        {
            List<Customer> customers = new List<Customer>();

            using(SqlCommand cmd = new SqlCommand("FetchCustomers", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@offsetIndex", SqlDbType.Int).Value = offsetIndex;
                cmd.Parameters.Add("@entriesPerPage", SqlDbType.Int).Value =entriesPerPage;

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



        // Summary:
        //      Searches the database for a Customer's Name and returns the results
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


        public static DataTable FetchCustomersIntoDataTable(SqlConnection connection, int offsetIndex, int entriesPerPage)
        {
            DataTable customers = new DataTable();
            SqlCommand cmd = new SqlCommand("FetchCustomers", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@offsetIndex", SqlDbType.Int).Value = offsetIndex;
            cmd.Parameters.Add("@entriesPerPage", SqlDbType.Int).Value = entriesPerPage;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(customers);
            return customers;
        }
        #endregion SelectQueries


        public static void UpdateDB(SqlConnection conn, DataTable table)
        {

        }
    }
}

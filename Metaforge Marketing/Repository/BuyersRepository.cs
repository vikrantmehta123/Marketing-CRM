using Metaforge_Marketing.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Metaforge_Marketing.Repository
{
    public class BuyersRepository
    {
        #region Insert Queries
        public static void InsertToDB(SqlConnection conn, IEnumerable<Buyer> buyers)
        {
            using(SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Buyers (BuyerName, Email, Phone, CustId) VALUES (@name, @email, @phone, @custId)";
                foreach (Buyer buyer in buyers)
                {
                    cmd.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = buyer.Name;
                    cmd.Parameters.Add("@email", System.Data.SqlDbType.VarChar).Value = buyer.Email;
                    cmd.Parameters.Add("@custId", System.Data.SqlDbType.Int).Value = buyer.Customer.Id;
                    if (! String.IsNullOrEmpty(buyer.Phone)) { cmd.Parameters.Add("@phone", System.Data.SqlDbType.VarChar).Value =buyer.Phone; } 
                    else { cmd.Parameters.Add("@phone", System.Data.SqlDbType.VarChar).Value = DBNull.Value; }

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        #endregion Insert Queries

        #region Select Queries

        // Summary:
        //      Fetches all the buyers of a Customer
        //      Used when adding an RFQ
        // Parameters:
        //      Customer- Whose buyers need to be fetched
        public static IEnumerable<Buyer> FetchBuyers(SqlConnection conn, Customer cust)
        {
            List<Buyer> buyers = new List<Buyer>();
            using(SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Buyers WHERE CustId = @custId";
                cmd.Parameters.Add("@custId", System.Data.SqlDbType.Int).Value = cust.Id;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Buyer buyer = new Buyer
                    {
                        Name = reader["BuyerName"].ToString(),
                        Id = Convert.ToInt32(reader["Id"]),
                        Email = reader["Email"].ToString()
                    };
                    buyers.Add(buyer);
                }
            }
            return buyers;
        }
        #endregion Select Queries
    }
}

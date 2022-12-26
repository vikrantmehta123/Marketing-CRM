

using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;

namespace Metaforge_Marketing.Repository
{
    public class AdminsRepository
    {

        #region Insert Queries
        // Summary:
        //     Given an Admin, inserts him in the database.
        // Parameters:
        //     Requires an open connection, and an instance of Admin class to insert 
        public static void InsertToDB(SqlConnection conn, Admin admin)
        {
            SqlCommand cmd = new SqlCommand("InsertAdmin", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = admin.Name;
            cmd.ExecuteNonQuery();
        }

        #endregion Insert Queries


        #region Select Queries
        // Summary:
        //      Fetches all the admins from the database. Returns them as IEnumerable<Item>
        //  Parameters: 
        //      Requires an open connection
        public static IEnumerable<Admin> FetchAdmins(SqlConnection conn)
        {
            List<Admin> admins = new List<Admin>();
            SqlCommand cmd = new SqlCommand("FetchAdmins", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                admins.Add(new Admin
                {
                    Name = reader["Name"].ToString(),
                    Id = Convert.ToInt32(reader["Id"])
                });
            }
            reader.Close();
            return admins;
        }

        public static IEnumerable<Admin> FetchPerformanceReview(SqlConnection conn, DateTime startDate, DateTime endDate)
        {
            List<Admin> list = new List<Admin>();
            SqlCommand cmd = new SqlCommand("FetchAmountOfBusinessReceived", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataReader reader= cmd.ExecuteReader();
            while (reader.Read())
            {
                Admin admin = new Admin 
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    RFQCount = Convert.ToInt32(reader["RFQCount"]),
                };

                if (reader["CostingPreparedCount"] != DBNull.Value) { admin.PreparedCostingsCount = Convert.ToInt16(reader["CostingPreparedCount"]); }
                else { admin.PreparedCostingsCount = 0; }

                if (reader["ConvertedItemsCount"] != DBNull.Value) { admin.ConvertedQuotationsCount = Convert.ToInt16(reader["ConvertedItemsCount"]); }
                else { admin.ConvertedQuotationsCount = 0; }

                if (reader["AvgResponseTime"] != DBNull.Value) { admin.AvgResponseTime = (float)Convert.ToDecimal(reader["AvgResponseTime"]); }
                else { admin.AvgResponseTime = -1; }
                list.Add(admin);
            }
            reader.Close();
            return list;
        }

        #endregion Select Queries
    }
}

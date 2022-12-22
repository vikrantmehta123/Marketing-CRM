

using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace Metaforge_Marketing.Repository
{
    public class AdminsRepository
    {
        /// <summary>
        /// Given an Admin, inserts him in the database.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="admin"></param>
        public static void InsertToDB(SqlConnection conn, Admin admin)
        {
            SqlCommand cmd = new SqlCommand("InsertAdmin", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = admin.Name;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Fetches all the Admins from the database
        /// </summary>
        /// <param name="conn"></param>
        /// <returns>A list of the admins present in the database</returns>
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

        /// <summary>
        /// NOT TESTED YET:
        /// Supposed to return the count of the Items whose quotations were prepared by a given admin
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="admin"></param>
        /// <returns>Int - count of the quotations prepared by that admin</returns>
        public static int CountPreparedQuotations(SqlConnection conn, Admin admin, DateTime startDate, DateTime endDate)
        {
            int count;
            using(SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = 
                    "SELECT COUNT(t.Id) FROM ( " +
                    "SELECT Items.Id AS Id FROM Items " +
                    "LEFT JOIN " +
                    "RFQs ON RFQs.Id = Items.RFQId " +
                    "WHERE RFQs.EnquiryDate > @startDate AND RFQs.EnquiryDate < @endDate AND AdminId = @adminId ) t";
                cmd.Parameters.Add("@adminId", System.Data.SqlDbType.Int).Value = admin.Id;
                cmd.Parameters.Add("@startDate", System.Data.SqlDbType.Date).Value = startDate.Date;
                cmd.Parameters.Add("@endDate", System.Data.SqlDbType.Date).Value = endDate.Date;
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return count;
        }

        /// <summary>
        /// Given an Admin, counts the number of rfqs that were brought by the Admin within a given period
        /// Requires that the startdate and the enddate be given as DateTime objects
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="admin"></param>
        /// <returns>Int- count of the RFQs that were brought in by the admin</returns>
        public static int CountRFQs(SqlConnection conn, Admin admin, DateTime startDate, DateTime endDate)
        {
            int count;
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(Id) FROM RFQs WHERE AdminId = @adminId AND EnquiryDate >= @startDate AND EnquiryDate <= @endDate";
                cmd.Parameters.Add("@adminId", System.Data.SqlDbType.Int).Value = admin.Id;
                cmd.Parameters.Add("@startDate", System.Data.SqlDbType.Date).Value = startDate.Date;
                cmd.Parameters.Add("@endDate", System.Data.SqlDbType.Date).Value = endDate.Date;
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return count;
        }


        public static int CountConvertedQuotations(SqlConnection conn, Admin admin, DateTime startDate, DateTime endDate)
        {
            int count;
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT COUNT(t.Id) FROM ( " +
                    "SELECT Items.Id AS Id FROM Items " +
                    "LEFT JOIN " +
                    "RFQs ON RFQs.Id = Items.RFQId " +
                    "WHERE EnquiryDate >= @startDate AND EnquiryDate <= @endDate AND Items.Status = @status AND RFQs.AdminId = @adminId ) t";
                cmd.Parameters.Add("@adminId", System.Data.SqlDbType.Int).Value = admin.Id;
                cmd.Parameters.Add("@startDate", System.Data.SqlDbType.Date).Value = startDate.Date;
                cmd.Parameters.Add("@endDate", System.Data.SqlDbType.Date).Value = endDate.Date;
                cmd.Parameters.Add("@status", System.Data.SqlDbType.Int).Value = ((int)ItemStatusEnum.POReceived);
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return count;
        }


        // TODO: Implement the function to compute the average response time, given an admin and the start and end date
        public static int ComputeAvgResponseTime(SqlConnection conn, Admin admin)
        {
            int avgResponseTime = 0;
            using(SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT AVG()";
            }
            return avgResponseTime;
        }
    }
}



using Metaforge_Marketing.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

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
    }
}



using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace Metaforge_Marketing.Repository
{
    public class RMRepository
    {
        #region Select Queries

        // Summary:
        //      Fetches a list of Raw Materials in the master data.
        //      These are the raw materials that are used by Metaforge
        public static IEnumerable<RM> FetchRMs(SqlConnection connection)
        {
            List<RM> RMs= new List<RM>();

            using(SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM RMMaster";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RM rm = new RM
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        CurrentRate = (float)Convert.ToDouble(reader["CurrentRate"]),
                        Grade = (reader["Grade"]).ToString(),
                        Category = (RMCategoryEnum)Convert.ToInt16(reader["Category"])
                    };
                    RMs.Add(rm);
                }
                reader.Close();
            }
            return RMs;
        }


        public static DataTable FetchRMsTable(SqlConnection conn)
        {
            DataTable table= new DataTable();
            string query = "SELECT * FROM RMMaster";
            using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
            {
                da.Fill(table);
            }
            return table;
        }
        #endregion Select Queries

        #region Update Queries

        public static void UpdateRMMaster(SqlConnection conn, DataTable table)
        {
            using(SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM RMMaster", conn))
            {
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Update(table);
                
            }
        }
        #endregion Update Queries

        #region Insert Queries
        public static void InsertToDB(SqlConnection conn, RM RMToInsert)
        {
            using(SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO RMMaster (Grade, Category, CurrentRate) VALUES (@grade, @category, @currentRate)";
                cmd.Parameters.Add("@grade", SqlDbType.VarChar).Value = RMToInsert.Grade;
                cmd.Parameters.Add("@currentRate", SqlDbType.Float).Value = (RMToInsert.CurrentRate);
                cmd.Parameters.Add("@category", SqlDbType.Int).Value = ((int)RMToInsert.Category); 
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
        #endregion Insert Queries
    }
}

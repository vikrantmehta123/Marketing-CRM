

using Metaforge_Marketing.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Metaforge_Marketing.Repository
{
    public class OperationsRepository
    {
        #region Master Sheet Queries
        public static DataTable FetchOperationsIntoDatatable(SqlConnection conn)
        {
            DataTable table = new DataTable();
            string query = "SELECT * FROM Operations";
            using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
            {
                da.Fill(table);
            }
            return table;
        }

        public static void UpdateMaster(SqlConnection conn, DataTable table)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Operations", conn))
            {
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Update(table);
            }
        }

        #endregion Master Sheet Queries

        // Summary:
        //      Fetches list of all operations present in the database
        public static IEnumerable<Operation> FetchOperations (SqlConnection conn)
        {
            var operations = new List<Operation> ();
            using(SqlCommand cmd = conn.CreateCommand ())
            {
                cmd.CommandText = "SELECT * FROM Operations";
                SqlDataReader reader = cmd.ExecuteReader ();
                while (reader.Read ())
                {
                    Operation op = new Operation()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        OperationName = reader["Name"].ToString(),
                    };
                    operations.Add(op);
                }
                reader.Close();
                return operations;
            }
        }
    }
}

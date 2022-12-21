

using Metaforge_Marketing.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Metaforge_Marketing.Repository
{
    public class OperationsRepository
    {

        public static IEnumerable<Operation> FetchOperations (SqlConnection conn)
        {
            var operations = new List<Operation> ();
            using(SqlCommand cmd = conn.CreateCommand ())
            {
                cmd.CommandText = "SELECT * FROM Operations";
                SqlDataReader reader = cmd.ExecuteReader ();
                while (reader.Read ())
                {
                    operations.Add(new Operation
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        OperationName = reader["Name"].ToString (),
                    });
                }
                reader.Close();
                return operations;
            }
        }
    }
}

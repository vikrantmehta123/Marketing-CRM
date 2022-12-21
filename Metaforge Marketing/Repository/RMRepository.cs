

using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Metaforge_Marketing.Repository
{
    public class RMRepository
    {
        public static IEnumerable<RM> FetchRMs(SqlConnection connection)
        {
            List<RM> RMs= new List<RM>();

            using(SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM RMMaster";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RM rm = new RM();
                    rm.Id = Convert.ToInt32(reader["Id"]);
                    rm.CurrentRate = (float)Convert.ToDouble(reader["CurrentRate"]);
                    rm.Grade = (reader["Grade"]).ToString();
                    rm.Category = (RMCategoryEnum)Convert.ToInt16(reader["Category"]);
                    RMs.Add(rm);
                }
                reader.Close();
            }
            return RMs;
        }
    }
}

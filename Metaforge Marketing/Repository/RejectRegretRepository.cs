using Metaforge_Marketing.Models;
using Microsoft.Data.SqlClient;
using System;

namespace Metaforge_Marketing.Repository
{
    public class RejectRegretRepository
    {
        public static void Insert(SqlConnection conn, SqlTransaction transaction, Item item)
        {
            SqlCommand cmd = new SqlCommand("InsertRejectRegret", conn, transaction)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@itemId", System.Data.SqlDbType.Int).Value= item.Id;
            cmd.Parameters.Add("@reason", System.Data.SqlDbType.VarChar).Value = item.Reason;
            cmd.Parameters.Add("@date", System.Data.SqlDbType.Date).Value = DateTime.Today.Date;
            cmd.Parameters.Add("@category", System.Data.SqlDbType.Int).Value = ((int)item.RejectRegret);
            cmd.ExecuteNonQuery();
        }
    }
}

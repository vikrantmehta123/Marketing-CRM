
using System;
using System.Data;
using System.Windows;
using Metaforge_Marketing.Models;
using Microsoft.Data.SqlClient;

namespace Metaforge_Marketing.Repository
{
    public class PORepository
    {
        // Summary:
        //      Inserts a PO in the database
        public static void Insert(SqlConnection conn, SqlTransaction transaction, PO po, Item item)
        {
            SqlCommand cmd = new SqlCommand("InsertPO", conn, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = po.Qty;
            cmd.Parameters.Add("@quotedRate", SqlDbType.Decimal).Value = po.QuotedRate;
            cmd.Parameters.Add("@approvedRate", SqlDbType.Decimal).Value = po.ApprovedRate;
            cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = item.Id;
            cmd.Parameters.Add("@date", SqlDbType.Date).Value = po.Date.Date;
            cmd.Parameters.Add("@poNumber", SqlDbType.VarChar).Value = po.Number;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
            
        }
    }
}

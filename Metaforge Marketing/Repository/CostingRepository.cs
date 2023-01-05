
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace Metaforge_Marketing.Repository
{
    public class CostingRepository
    {

        #region Insert Queries
        // TODO: Use this query if you implement Select Admin on the costing page.
        private static void SetAdmin(SqlConnection conn, SqlTransaction transaction, Item item, Admin admin)
        {
            string query = "UPDATE Items SET AdminId = @adminId WHERE Id = @itemId";
            SqlCommand cmd = new SqlCommand(query, conn, transaction);
            cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = item.Id;
            cmd.Parameters.Add("@adminId", SqlDbType.Int).Value = admin.Id;
        }


        // Summary:
        //      Updates the status value of an Item
        public static void UpdateItemStatus(SqlConnection conn, SqlTransaction transaction, Item item, ItemStatusEnum status)
        {
            SqlCommand cmd = new SqlCommand("UpdateItemStatus")
            {
                Connection = conn,
                Transaction = transaction,
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = item.Id;
            cmd.Parameters.Add("@status", SqlDbType.Int).Value = ((int)status);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion Insert Queries

        #region Select Queries

        // Summary:
        //      Fetches Raw Material costings of an Item into a Datatable. 
        //      Used in Quotation Comparison Reports
        public static DataTable FetchRMCostingsIntoDatatable(SqlConnection conn, Item item)
        {
            DataTable table = new DataTable();
            SqlCommand cmd = new SqlCommand("SELECT RMCostings.*, RMMaster.* FROM RMCostings INNER JOIN RMMaster ON RMMasterId = RMMaster.Id WHERE ItemId = @itemId", conn);
            cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = item.Id;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(table);
            return table;
        }

        // Summary:
        //      Fetches the Conversion Costings into a table based on the item and the category
        //      Used in Quotation Comparison
        public static DataTable FetchCCIntoDatatable(SqlConnection conn, Item item, CostingCategoryEnum category)
        {
            DataTable table = new DataTable();
            SqlCommand cmd = new SqlCommand("SELECT ConversionCostings.*, Operations.* FROM ConversionCostings INNER JOIN Operations ON OperationId = Operations.Id WHERE ItemId = @itemId AND WhoseCosting = @category ", conn);
            cmd.Parameters.Add("@itemId",SqlDbType.Int).Value = item.Id;
            cmd.Parameters.Add("@category", SqlDbType.Int).Value = ((int)category);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(table);
            return table;
        }

        #endregion Select Queries
    }
}

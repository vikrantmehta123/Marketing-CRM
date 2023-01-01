
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
namespace Metaforge_Marketing.Repository
{
    public class CostingRepository
    {

        #region Insert Queries

        public static void InsertItemHistory(SqlConnection conn, Costing costing, SqlTransaction transaction)
        {
            SqlCommand ItemHistoryCommand = new SqlCommand("InsertItemHistory", conn, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            // Add Parameters to the Item History command
            ItemHistoryCommand.Parameters.Add("@itemId", SqlDbType.Int).Value = costing.Item.Id;
            ItemHistoryCommand.Parameters.Add("@oldStatus", SqlDbType.Int).Value = ((int)costing.Item.Status);
            ItemHistoryCommand.Parameters.Add("@newStatus", SqlDbType.Int).Value = costing.ComputeItemStatus();
            ItemHistoryCommand.Parameters.Add("@date", SqlDbType.Int).Value = costing.Remark.EventDate;
            ItemHistoryCommand.Parameters.Add("@note", SqlDbType.Int).Value = costing.Remark.Note;

            // Execute the insert and clear parameters
            ItemHistoryCommand.ExecuteNonQuery();
            ItemHistoryCommand.Parameters.Clear();
        }

        #endregion Insert Queries

        #region Select Queries
        // Summary:
        //      Fetches the RMCosting of an Item
        //      Returns an empty instance if no costing is present
        // Parameters:
        //      item- the item whose costing needs to be fetched
        //      category- the category of the costing: Customer's, Metaforge's, Customer's approved?
        public static RMCosting FetchRMCosting(SqlConnection conn, Item selectedItem, CostingCategoryEnum category, RMCosting rmCosting)
        {
            SqlCommand cmd = new SqlCommand("FetchRMCosting", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = selectedItem.Id;
            cmd.Parameters.Add("@whoseCosting", SqlDbType.Int).Value = ((int)category);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                rmCosting.IsRMCostingPresent = true;
                while (reader.Read())
                {
                    rmCosting.Id = Convert.ToInt32(reader["Id"]);
                    rmCosting.RMAsPerDrawing = reader["RMAsPerDrawing"].ToString();
                    rmCosting.CostPerPiece = (float)Convert.ToDecimal(reader["RMCostPerPiece"]);
                    rmCosting.RMRate = (float)Convert.ToDecimal(reader["RMRate"]);
                    rmCosting.RMConsidered = new RM()
                    {
                        Id = Convert.ToInt32(reader["RMMasterId"]),
                        Grade = reader["Grade"].ToString(),
                            
                        Category = (RMCategoryEnum)Convert.ToInt16(reader["Category"]),
                        CurrentRate = (float)Convert.ToDecimal(reader["CurrentRate"])
                    };
                }
            }
            reader.Close();
            return rmCosting;
        }


        // Summary:
        //      Fetches the Conversion Costings from the Database, in the ascending order of the Step no and returns those as a list
        //      Returns an empty instance if no result is present in the database for given params
        // Parameters:
        //      Item- The Item whose Conversion Costings need to be fetched
        //      CostingCategory- Which Costing to Fetch? Customer's, Metaforge's, or Customer's Approved
        public static ConversionCosting FetchConversionCosting(SqlConnection conn, Item selectedItem, CostingCategoryEnum category)
        {
            ConversionCosting convCosting = new ConversionCosting();
            List<Operation> operations = new List<Operation>();

            SqlCommand cmd = new SqlCommand("FetchConversionCosting", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = selectedItem.Id;
            cmd.Parameters.Add("@whoseCosting", SqlDbType.Int).Value = ((int)category);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                convCosting.IsConvCostingPresent= true;
                while(reader.Read())
                {
                    Operation op = new Operation
                    {
                        OperationName = reader["OperationName"].ToString(),
                        CostPerPiece = (float)Convert.ToDecimal(reader["CCPerPiece"]),
                        Id = Convert.ToInt32(reader["Id"]),
                        IsOutsourced = Convert.ToBoolean(reader["IsOutsourced"]),
                        StepNo = Convert.ToInt32(reader["StepNo"]),
                    };
                    operations.Add(op);
                }
            }
            else
            {
                convCosting.IsConvCostingPresent = false;
            }
            reader.Close();
            convCosting.Operations = new System.Collections.ObjectModel.ObservableCollection<Operation>( operations);
            return convCosting;
        }


        // Summary:
        //      Fetches the Raw Material and The Conversion Costing of an Item
        // Parameters:
        //      item- The Item whose costing needs to be fetched
        //      category- the Category of the costing: Customer's, Metaforge's, or Customer's Approved costing
        public static Costing FetchCosting(SqlConnection conn, Item item, CostingCategoryEnum category)
        {
            Costing costing = new Costing
            {
                
                RMCosting = FetchRMCosting(conn, item, category, new RMCosting()),
                Item = item,
                Category = category,
                ConvCosting = FetchConversionCosting(conn, item, category)
            };
            return costing;
        }

        // Summary:
        //      Fetches all the costings of an RFQ, and returns those costings as a list
        // Parameters:
        //      rfq- The RFQ whose items will be fetched, and the items' costings will be returned
        //      category- The category of the costing
        public static IEnumerable<Costing> FetchCostings(SqlConnection conn, RFQ rfq, CostingCategoryEnum category)
        {
            List<Costing> results = new List<Costing>();
            foreach (Item item in rfq.Items)
            {
                Costing costing = FetchCosting(conn, item, category);
                results.Add(costing);
            }
            return results;
        }


        // Summary:
        //      Fetches Raw Material costings of an Item into a Datatable. 
        //      Used in Costing Comparison Reports
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
        //      Used in Costing Comparison
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

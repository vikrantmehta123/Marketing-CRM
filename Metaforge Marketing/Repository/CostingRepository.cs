
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
        /// <summary>
        /// Inserts the whole Costing into their respective tables
        /// Requires that the data be valid
        /// The Operation is atomic
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="costing"></param>
        public static void InsertToDB(SqlConnection conn, Costing costing)
        {
            SqlTransaction transaction = conn.BeginTransaction();
            try
            {
                int RMCostingId = InsertRMCosting(conn, costing, transaction);
                foreach (Operation op in costing.Operations)
                {
                    int CCId = InsertConversionCosting(conn, op, costing, transaction);
                }
                InsertItemHistory(conn, costing, transaction);
                UpdateItemStatus(conn, costing, transaction);
                transaction.Commit();
            }
            catch(Exception e1) 
            {
                MessageBox.Show(e1.Message);
                try
                {
                    transaction.Rollback(); 
                }
                catch(Exception e2)
                {
                    MessageBox.Show(e2.Message);
                }
            }
            finally { transaction.Dispose(); }


        }
        public static int InsertRMCosting(SqlConnection conn, Costing costing, SqlTransaction transaction)
        {
            SqlCommand RMCostingCommand = new SqlCommand("InsertRMCosting", conn, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            // Add Parameters for the RM Costing Query
            RMCostingCommand.Parameters.Add("@rmRate", SqlDbType.Float).Value = costing.RMCosting.RMRate;
            RMCostingCommand.Parameters.Add("@rmAsPerDrawing", SqlDbType.Float).Value = costing.RMCosting.RMAsPerDrawing;
            RMCostingCommand.Parameters.Add("@rmCostPerPiece", SqlDbType.Float).Value = costing.RMCosting.CostPerPiece;
            RMCostingCommand.Parameters.Add("@whoseCosting", SqlDbType.Int).Value = ((int)costing.Category);
            RMCostingCommand.Parameters.Add("@itemId", SqlDbType.Int).Value = costing.Item.Id;
            RMCostingCommand.Parameters.Add("@rmMasterId", SqlDbType.Int).Value = costing.RMCosting.RMConsidered.Id;

            // Execute the insert and clear parameters
            int Id = Convert.ToInt32(RMCostingCommand.ExecuteScalar());
            RMCostingCommand.Parameters.Clear();
            return Id;
        }

        /// <summary>
        /// Given a Costing, and the Operation, 
        /// Inserts "ONE" operation in the database and returns the inserted row's Id
        /// Used when inserting costings of Items
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="op"></param>
        /// <param name="costing"></param>
        /// <returns></returns>
        public static int InsertConversionCosting(SqlConnection conn, Operation op, Costing costing, SqlTransaction transaction)
        {
            SqlCommand ConvCostingCommand = new SqlCommand("InsertConversionCosting", conn, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            // Add the paramters of the query
            ConvCostingCommand.Parameters.Add("@ccPerPiece", SqlDbType.Float).Value = op.CostPerPiece;
            ConvCostingCommand.Parameters.Add("@whoseCosting", SqlDbType.Int).Value = ((int)costing.Category);
            ConvCostingCommand.Parameters.Add("@itemId", SqlDbType.Int).Value = costing.Item.Id;
            ConvCostingCommand.Parameters.Add("@operationId", SqlDbType.Int).Value = op.Id;
            ConvCostingCommand.Parameters.Add("@stepNo", SqlDbType.Int).Value = costing.Operations.IndexOf(op) + 1;
            ConvCostingCommand.Parameters.Add("@isOutsourced", SqlDbType.Int).Value = Convert.ToInt16(op.IsOutsourced);
            ConvCostingCommand.Parameters.Add("@adminId", SqlDbType.Int).Value = costing.CostingPreparedBy.Id;

            // Execute the query and clear parameters
            int Id = Convert.ToInt32(ConvCostingCommand.ExecuteScalar());
            ConvCostingCommand.Parameters.Clear();

            return Id;
        }

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


        // Summary:
        //      Updates an Item's status in the database
        public static void UpdateItemStatus(SqlConnection conn, Costing costing, SqlTransaction transaction)
        {
            SqlCommand UpdateItemStatusCommand = new SqlCommand("UpdateItemStatus", conn, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            // Add Parameters for the Update Status Command
            UpdateItemStatusCommand.Parameters.Add("@itemId", SqlDbType.Int).Value = costing.Item.Id;
            UpdateItemStatusCommand.Parameters.Add("@status", SqlDbType.Int).Value = costing.ComputeItemStatus();

            UpdateItemStatusCommand.ExecuteNonQuery();
            UpdateItemStatusCommand.Parameters.Clear();
        }

        #region Select Queries
        // Summary:
        //      Fetches the RMCosting of an Item
        //      Returns an empty instance if no costing is present
        // Parameters:
        //      item- the item whose costing needs to be fetched
        //      category- the category of the costing: Customer's, Metaforge's, Customer's approved?
        public static RMCosting FetchRMCosting(SqlConnection conn, Item selectedItem, CostingCategoryEnum category)
        {
            RMCosting rmCosting = new RMCosting();
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
                    rmCosting.RMConsidered = new RM
                    {
                        Id = Convert.ToInt32(reader["RMMasterId"]),
                        Grade = reader["Grade"].ToString(),
                        Category = (RMCategoryEnum)Convert.ToInt16(reader["Category"]), 
                        CurrentRate = (float)Convert.ToDecimal(reader["CurrentRate"])
                    };
                    rmCosting.RMRate = (float)Convert.ToDecimal(reader["RMRate"]);

                    if (reader["DetailsId"] != DBNull.Value)
                    {
                        rmCosting.IsRMCostingDetailsPresent = true;
                        rmCosting.ScrapRate = (float)Convert.ToDecimal(reader["ScrapRate"]);
                        rmCosting.ScrapRecovery = (float)Convert.ToDecimal(reader["ScrapRecovery"]);
                        rmCosting.CuttingAllowance = (float)Convert.ToDecimal(reader["CuttingAllowance"]);
                    }
                }
                reader.Close();
            }
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
                        CostPerPiece = (float)Convert.ToDecimal(reader["CCPerPiece"]),
                        Id = Convert.ToInt32(reader["Id"]),
                        IsOutsourced = Convert.ToBoolean(reader["IsOutsourced"]),
                        StepNo = Convert.ToInt32(reader["StepNo"]),
                        Machine = new Machine ()
                    };
                    // If there Conversion Costing Details such as machine description present, assign them
                    if (reader["MachineDescription"] != DBNull.Value) 
                    { 
                        op.Machine.MachineDescription = reader["MachineDescription"].ToString();
                        if (! convCosting.IsConvCostingDetailsPresent) { convCosting.IsConvCostingDetailsPresent = true; }
                    };
                    if (reader["MachineName"] != DBNull.Value) { op.Machine.MachineName = reader["MachineName"].ToString(); }
                    if (reader["Efficiency"] != DBNull.Value) { op.Machine.Efficiency = (float)Convert.ToDecimal(reader["Efficiency"]); }
                    if (reader["CycleTime"] != DBNull.Value) { op.Machine.CycleTime = Convert.ToInt16(reader["CycleTime"]); }
                    if (reader["MCHr"] != DBNull.Value) { op.Machine.MCHr = (float)Convert.ToDecimal(reader["MCHr"]); }

                    operations.Add(op);
                }
                reader.Close();
            }
            else
            {
                convCosting.IsConvCostingDetailsPresent = false;
                convCosting.IsConvCostingPresent = false;
            }
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
                RMCosting = FetchRMCosting(conn, item, category),
                Item = item,
                Category = category
            };
            costing.ConvCosting = FetchConversionCosting(conn, item, category);
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM RMCostings LEFT JOIN RMCostingDetails ON RMCostings.Id = RMCostingDetails.RMCostingId WHERE ItemId = @itemId", conn);
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM ConversionCostings LEFT JOIN CCDetails ON CCId = ConversionCostings.Id WHERE ItemId = @itemId AND WhoseCosting = @category ", conn);
            cmd.Parameters.Add("@itemId",SqlDbType.Int).Value = item.Id;
            cmd.Parameters.Add("@category", SqlDbType.Int).Value = ((int)category);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(table);
            return table;
        }

        #endregion Select Queries
    }
}

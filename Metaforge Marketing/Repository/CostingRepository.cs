
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

        // Summary:
        //      Given a DataTable, Inserts/ updates/ deletes rows that are changed
        // Parameters:
        //      Item- whose Costings are being inserted
        //      category- the Category of the costing
        //      DataTable- The datatable from which the changes are to be taken
        private static void InsertCC(SqlConnection conn, SqlTransaction transaction, DataTable table, Item item, CostingCategoryEnum category)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                SqlCommand InsertCommand = new SqlCommand("InsertConversionCosting", conn)
                {
                    CommandType = CommandType.StoredProcedure,
                    Transaction = transaction,
                    Connection = conn
                };

                adapter.InsertCommand = InsertCommand;
                InsertCommand.Parameters.Add("@itemId", SqlDbType.Int).Value = item.Id;
                InsertCommand.Parameters.Add("@whoseCosting", SqlDbType.Int).Value = ((int)category);
                InsertCommand.Parameters.Add("@operationId", SqlDbType.Int, 16, "OperationId");
                InsertCommand.Parameters.Add("@stepNo", SqlDbType.Int, 16, "StepNo");
                InsertCommand.Parameters.Add("@ccPerPiece", SqlDbType.Decimal, 16, "CCPerPiece");
                InsertCommand.Parameters.Add("@isOutsourced", SqlDbType.Int, 16, "IsOutsourced");

                adapter.DeleteCommand = new SqlCommand("DELETE FROM ConversionCostings WHERE Id = @id")
                {
                    Connection = conn,
                    Transaction = transaction
                };
                adapter.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 32, "Id");

                SqlCommand UpdateCommand = new SqlCommand("UpdateConversionCosting", conn, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                };
                UpdateCommand.Parameters.Add("@whoseCosting", SqlDbType.Int).Value = ((int)category);
                UpdateCommand.Parameters.Add("@operationId", SqlDbType.Int, 16, "OperationId");
                UpdateCommand.Parameters.Add("@stepNo", SqlDbType.Int, 16, "StepNo");
                UpdateCommand.Parameters.Add("@ccPerPiece", SqlDbType.Decimal, 16, "CCPerPiece");
                UpdateCommand.Parameters.Add("@isOutsourced", SqlDbType.Int, 16, "IsOutsourced");

                adapter.UpdateCommand = UpdateCommand;
                adapter.UpdateCommand.Parameters.Add("@id", SqlDbType.Int, 32, "Id");

                try
                {
                    adapter.Update(table);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private static void InsertRM(SqlConnection conn, SqlTransaction transaction, Item item, RMCosting costing, CostingCategoryEnum category)
        {
            SqlCommand InsertCommand = new SqlCommand("InsertRMCosting", conn)
            {
                CommandType = CommandType.StoredProcedure,
                Connection = conn,
                Transaction = transaction
            };
            // Add insert query
            InsertCommand.Parameters.Add("@rmMasterId", SqlDbType.Int).Value = costing.RMConsidered.Id;
            InsertCommand.Parameters.Add("@whoseCosting", SqlDbType.Int).Value = ((int)category);
            InsertCommand.Parameters.Add("@rmCostPerPiece", SqlDbType.Decimal).Value = costing.ComputeRMCost(item);
            InsertCommand.Parameters.Add("@rmAsPerDrawing", SqlDbType.VarChar).Value = costing.RMAsPerDrawing;
            InsertCommand.Parameters.Add("@rmRate", SqlDbType.Decimal).Value = costing.RMRate;
            InsertCommand.Parameters.Add("@itemId", SqlDbType.Int).Value = item.Id;

            try
            {
                InsertCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // Summary:
        //      Updates a Raw Material entry if required
        private static void UpdateRM(SqlConnection conn, SqlTransaction transaction, Item item, RMCosting costing, CostingCategoryEnum category)
        {
            SqlCommand UpdateCommand = new SqlCommand("UpdateRMCosting")
            {
                CommandType = CommandType.StoredProcedure,
                Connection = conn,
                Transaction = transaction
            };
            UpdateCommand.Parameters.Add("@id", SqlDbType.Int).Value = costing.Id;
            UpdateCommand.Parameters.Add("@rmMasterId", SqlDbType.Int).Value = costing.RMConsidered.Id;
            UpdateCommand.Parameters.Add("@rmCostPerPiece", SqlDbType.Decimal).Value = costing.CostPerPiece;
            UpdateCommand.Parameters.Add("@rmRate", SqlDbType.Decimal).Value = costing.RMRate;
            UpdateCommand.Parameters.Add("@whoseCosting", SqlDbType.Int).Value = ((int)category);
            UpdateCommand.Parameters.Add("@itemId", SqlDbType.Int).Value = item.Id;
            UpdateCommand.Parameters.Add("@rmAsPerDrawing", SqlDbType.VarChar).Value = costing.RMAsPerDrawing;
            try
            {
                UpdateCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

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

        public static void InsertItemHistory(SqlConnection conn, SqlTransaction transaction, Item item, ItemStatusEnum newStatus)
        {
            SqlCommand ItemHistoryCommand = new SqlCommand("InsertItemHistory", conn, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            // Add Parameters to the Item History command
            ItemHistoryCommand.Parameters.Add("@itemId", SqlDbType.Int).Value = item.Id;
            ItemHistoryCommand.Parameters.Add("@oldStatus", SqlDbType.Int).Value = ((int)item.Status);
            ItemHistoryCommand.Parameters.Add("@newStatus", SqlDbType.Int).Value = newStatus;
            ItemHistoryCommand.Parameters.Add("@date", SqlDbType.Date).Value = item.EventDate.Date;
            ItemHistoryCommand.Parameters.Add("@note", SqlDbType.VarChar).Value = item.GetNote(newStatus);

            // Execute the insert and clear parameters
            ItemHistoryCommand.ExecuteNonQuery();
            ItemHistoryCommand.Parameters.Clear();
        }

        public static void InsertCosting(SqlConnection conn, DataTable convCostingTable, Item item, RMCosting costing, CostingCategoryEnum category)
        {
            ItemStatusEnum status;
            if (category == CostingCategoryEnum.Metaforge) { status = ItemStatusEnum.MF_Costing_Prepared; }
            else if (category == CostingCategoryEnum.CustomerQuoted) { status = ItemStatusEnum.Customer_Costing_Prepared; }
            else { status = ItemStatusEnum.POReceived; }
            SqlTransaction transaction = conn.BeginTransaction();

            try
            {
                // TODO: Uncomment the below code if you keep Select Admin in the costing view
                //SetAdmin(conn, transaction, item, admin);
                if (costing.IsRMCostingPresent) { UpdateRM(conn, transaction, item, costing, category); }
                else { InsertRM(conn, transaction, item, costing, category); }
                InsertCC(conn, transaction, convCostingTable, item, category);

                if (status != item.Status) // Insert into Item History, only if it is a new entry
                {
                    InsertItemHistory(conn, transaction, item, status);
                }

                if (status > item.Status)
                {
                    UpdateItemStatus(conn, transaction, item, status);
                }

                transaction.Commit();
                MessageBox.Show("Success!");
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
                try
                {
                    transaction.Rollback();
                }
                catch (Exception e2)
                {
                    MessageBox.Show(e2.Message);
                }
            }
            finally
            {
                transaction.Dispose();
            }
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
        public static IEnumerable<Costing> FetchCostings(SqlConnection conn, IEnumerable<Item> items, CostingCategoryEnum category)
        {
            List<Costing> results = new List<Costing>();
            foreach (Item item in items)
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

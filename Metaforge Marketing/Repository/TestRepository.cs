﻿using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Transactions;
using System.Windows;

namespace Metaforge_Marketing.Repository
{
    public class TestRepository
    {
        #region Select Queries

        // Summary:
        //      Fetches a Conversion Costing record into a Datatable based on the Category
        //      Used in inserting/ updating costing
        public static DataTable FetchConvCosting(SqlConnection conn, Item item, CostingCategoryEnum category)
        {
            DataTable table = new DataTable();
            SqlCommand cmd = new SqlCommand("SELECT ConversionCostings.*, Operations.Name FROM ConversionCostings INNER JOIN Operations ON Operations.Id = OperationId WHERE ItemId = @itemId AND WhoseCosting = @whoseCosting ", conn);
            cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = item.Id;
            cmd.Parameters.Add("@whoseCosting", SqlDbType.Int).Value = ((int)category);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(table);
            return table;
        }

        #endregion Select Queries


        #region Insert Costing
        // Summary:
        //      Given a DataTable, Inserts/ updates/ deletes rows that are changed
        // Parameters:
        //      Item- whose Costings are being inserted
        //      category- the Category of the costing
        //      DataTable- The datatable from which the changes are to be taken
        private static void InsertCC(SqlConnection conn, SqlTransaction transaction, DataTable table, Item item, CostingCategoryEnum category)
        {
            using(SqlDataAdapter adapter=new SqlDataAdapter())
            {
                SqlCommand InsertCommand = new SqlCommand("InsertConversionCosting", conn)
                {
                    CommandType = CommandType.StoredProcedure,
                    Transaction = transaction,
                    Connection = conn
                };

                adapter.InsertCommand = InsertCommand;
                InsertCommand.Parameters.Add("@itemId", SqlDbType.Int).Value=item.Id;
                InsertCommand.Parameters.Add("@whoseCosting", SqlDbType.Int).Value=((int)category);
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
                catch(Exception ex)
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
            catch(Exception e)
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
            catch(Exception e)
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
                else { InsertRM(conn, transaction, item, costing, category);  }
                InsertCC(conn, transaction, convCostingTable, item, category);

                if(status < item.Status)
                {
                    UpdateItemStatus(conn, transaction, item, status);
                }

                string ItemHistoryNote;
                if (status == ItemStatusEnum.MF_Costing_Prepared) { ItemHistoryNote = "MF Costing prepared"; }
                else if(status == ItemStatusEnum.Customer_Costing_Prepared) { ItemHistoryNote = "Customer Costing prepared"; }
                else { ItemHistoryNote = "Approved costing entered"; }
                //CostingRepository.InsertItemHistory(conn, transaction, item, ItemHistoryNote);
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
                catch(Exception e2)
                {
                    MessageBox.Show(e2.Message);
                }
            }
            finally
            {
                transaction.Dispose();
            }
        }
        #endregion Insert Costing
    }
}

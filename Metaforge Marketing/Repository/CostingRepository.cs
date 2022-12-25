
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
                int RMCostingId = InsertRMCosting(conn, costing);
                foreach (Operation op in costing.Operations)
                {
                    int CCId = InsertConversionCosting(conn, op, costing);
                }
                InsertItemHistory(conn, costing);
                UpdateItemStatus(conn, costing);
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
        public static int InsertRMCosting(SqlConnection conn, Costing costing)
        {
            SqlCommand RMCostingCommand = new SqlCommand("InsertRMCosting", conn)
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
            RMCostingCommand.Parameters.Add("@adminId", SqlDbType.Int).Value = costing.CostingPreparedBy.Id;

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
        public static int InsertConversionCosting(SqlConnection conn, Operation op, Costing costing)
        {
            SqlCommand ConvCostingCommand = new SqlCommand("InsertConversionCosting", conn)
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

        public static void InsertItemHistory(SqlConnection conn, Costing costing)
        {
            SqlCommand ItemHistoryCommand = new SqlCommand("InsertItemHistory", conn)
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


        public static void UpdateItemStatus(SqlConnection conn, Costing costing)
        {
            SqlCommand UpdateItemStatusCommand = new SqlCommand("UpdateItemStatus", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            // Add Parameters for the Update Status Command
            UpdateItemStatusCommand.Parameters.Add("@itemId", SqlDbType.Int).Value = costing.Item.Id;
            UpdateItemStatusCommand.Parameters.Add("@status", SqlDbType.Int).Value = costing.ComputeItemStatus();

            UpdateItemStatusCommand.ExecuteNonQuery();
            UpdateItemStatusCommand.Parameters.Clear();
        }

        /// <summary>
        /// Given an Item and the Costing Category(Metaforge, Customer, Customer Approved)
        /// Fetches the record in the database if any, returns empty instance if there are no records
        /// Used when preparing costing
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="selectedItem"></param>
        /// <param name="category"></param>
        /// <returns>Instance of RMCosting class</returns>
        public static RMCosting FetchRMCosting(SqlConnection conn, Item selectedItem, CostingCategoryEnum category)
        {
            RMCosting rmCosting = new RMCosting();
            SqlCommand cmd = new SqlCommand("FetchRMCosting", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = selectedItem.Id;
            cmd.Parameters.Add("@whoseCosting", SqlDbType.Int).Value = ((int)category);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    rmCosting.Id = Convert.ToInt32(reader["Id"]);
                    rmCosting.CostPerPiece = (float)Convert.ToDecimal(reader["RMCostPerPiece"]);
                    rmCosting.CostingPreparedBy = new Admin { 
                        Id = Convert.ToInt32(reader["AdminId"]) ,
                        Name = reader["Name"].ToString()
                    };

                    rmCosting.RMConsidered = new RM { 
                        Id = Convert.ToInt32(reader["RMMasterId"]), 
                        Grade = reader["Grade"].ToString() ,
                        Category = (RMCategoryEnum)Convert.ToInt16(reader["Category"])
                    };
                    // TODO: Figure out why the RMRate if set before the RMConsidered property was giving error
                    rmCosting.RMRate = (float)Convert.ToDecimal(reader["RMRate"]);
                    rmCosting.RMAsPerDrawing = reader["RMAsPerDrawing"].ToString();
                }
                reader.Close();
            }
            return rmCosting;
        }

        public static IEnumerable<Operation> FetchConversionCosting(SqlConnection conn, Item selectedItem, CostingCategoryEnum category)
        {
            List<Operation> results = new List<Operation>();
            SqlCommand cmd = new SqlCommand("FetchConversionCostings", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = selectedItem.Id;
            cmd.Parameters.Add("@whoseCosting", SqlDbType.Int).Value = ((int)category);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while(reader.Read())
                {
                    Operation op = new Operation
                    { 
                        CostPerPiece = (float)Convert.ToDecimal(reader["CCPerPiece"]), 
                        Id = Convert.ToInt32(reader["Id"]), 
                        IsOutsourced = Convert.ToBoolean(reader["IsOutsourced"]),
                        StepNo = Convert.ToInt32(reader["StepNo"])
                    };
                    results.Add(op);
                }
                reader.Close();
            }

            return results;
        }
    }
}

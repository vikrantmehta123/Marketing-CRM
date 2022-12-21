

using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Microsoft.Data.SqlClient;
using System;
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

            // HAVE TESTED THE SHORT FORMAT INSERT, BUT HAVE NOT TESTED THE DETAILED FORMAT INSERT

            using(SqlCommand cmd = conn.CreateCommand())
            {
                string RMCostingQuery = "INSERT INTO RMCostings (RMRate, RMCostPerPiece, WhoseCosting, ItemId, RMMasterId) " +
                    "VALUES (@rmRate, @rmCostPerPiece, @whoseCosting, @itemId, @rmMasterId)";

                string RMDetailsQuery = "INSERT INTO RMCostingDetails (ScrapRate, ScrapRecovery, CuttingAllowance, RMCostingId) " +
                                        "VALUES (@scrapRate, @scrapRecovery, @cuttingAllowance, @rmCostingId)";


                string CCQuery = "INSERT INTO ConversionCostings (CCPerPiece, WhoseCosting, ItemId, OperationId) VALUES (@ccPerPiece, @whoseCosting, @itemId, @operationId)";

                string ItemsOpsQuery = "INSERT INTO Items_Operations (ItemId, OperationId, StepNo, IsOutsourced) VALUES (@itemId, @operationId, @stepNo, @isOutsourced)";

                // Add Params of the update query
                string UpdateStatusQuery = "UPDATE Items SET Status = @status WHERE Items.Id = @itemId";
                if(costing.ComputeItemStatus() > 0) { cmd.Parameters.Add("@status", System.Data.SqlDbType.Int).Value = costing.ComputeItemStatus(); }

                string CCDetailsQuery = "INSERT INTO CCDetails (CycleTime, Efficiency, MCHr, MachineId, CCId) " +
                    "VALUES (@cycleTime, @efficiency, @mchr, @machineId, @ccId)";

                
                // TODO : THINK WHETHER MachineId IS REALLY A GOOD COLUMN TO HAVE. Would MachineName do better / Serve the purpose?
                SqlTransaction transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;
                cmd.Connection= conn;

                try
                {
                    cmd.CommandText = RMCostingQuery;
                    // Add Parameters for the RM Costing Query
                    cmd.Parameters.Add("@rmRate", System.Data.SqlDbType.Float).Value =  costing.RMCosting.RMRate;
                    cmd.Parameters.Add("@rmCostPerPiece", System.Data.SqlDbType.Float).Value = costing.RMCosting.CostPerPiece;
                    cmd.Parameters.Add("@whoseCosting", System.Data.SqlDbType.Int).Value = ((int)costing.Category);
                    cmd.Parameters.Add("@itemId", System.Data.SqlDbType.Int).Value = costing.Item.Id;
                    cmd.Parameters.Add("@rmMasterId", System.Data.SqlDbType.Int).Value = costing.RMCosting.RMConsidered.Id;

                    int RMCostingId = Convert.ToInt32(cmd.ExecuteScalar());
                    if (((int)costing.Format) == ((int)CostingFormatEnum.Long))
                    {
                        cmd.CommandText = RMDetailsQuery;
                        cmd.Parameters.Add("@scrapRate", System.Data.SqlDbType.Float).Value = costing.RMCosting.ScrapRate;
                        cmd.Parameters.Add("@scrapRecovery", System.Data.SqlDbType.Float).Value = costing.RMCosting.ScrapRecovery;
                        cmd.Parameters.Add("@cuttingAllowance", System.Data.SqlDbType.Float).Value = costing.RMCosting.CuttingAllowance;
                        cmd.Parameters.Add("@rmCostingId", System.Data.SqlDbType.Int).Value = RMCostingId;
                        cmd.ExecuteNonQuery();
                    }

                    cmd.Parameters.Clear();

                    foreach (Operation op in costing.Operations)
                    {
                        cmd.CommandText = CCQuery;

                        cmd.Parameters.Add("@ccPerPiece", System.Data.SqlDbType.Float).Value = op.CostPerPiece;
                        cmd.Parameters.Add("@whoseCosting", System.Data.SqlDbType.Int).Value = ((int)costing.Category);
                        cmd.Parameters.Add("@itemId", System.Data.SqlDbType.Int).Value = costing.Item.Id;
                        cmd.Parameters.Add("@operationId", System.Data.SqlDbType.Int).Value = op.Id;

                        int CCId = Convert.ToInt32(cmd.ExecuteScalar());
                        if (((int)costing.Format) == ((int)CostingFormatEnum.Long))
                        {
                            cmd.CommandText = CCDetailsQuery;
                            cmd.Parameters.Add("@cycleTime", SqlDbType.Float).Value = op.CycleTime;
                            cmd.Parameters.Add("@efficiency", SqlDbType.Float).Value = op.Efficiency;
                            cmd.Parameters.Add("@mchr", SqlDbType.Float).Value = op.MCHr;
                            cmd.Parameters.Add("@ccId", SqlDbType.Int).Value = CCId;
                            cmd.ExecuteNonQuery();
                        }

                        cmd.CommandText = ItemsOpsQuery;
                        cmd.Parameters.Add("@stepNo", SqlDbType.Int).Value = costing.Operations.IndexOf(op) + 1;
                        cmd.Parameters.Add("@isOutsourced", SqlDbType.Int).Value = Convert.ToInt16(op.IsOutsourced);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    cmd.Parameters.Clear();
                    if (costing.ComputeItemStatus() > 0)
                    {
                        cmd.CommandText = UpdateStatusQuery;
                        cmd.Parameters.Add("@status", System.Data.SqlDbType.Int).Value = costing.ComputeItemStatus();
                        cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = costing.Item.Id;
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Successfully committed");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
        }
    }
}



using Metaforge_Marketing.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Metaforge_Marketing.Repository
{
    public class RFQsRepository
    {
        public static IEnumerable<RFQ> FetchRFQs(SqlConnection connection, int rfqStatus, int offsetIndex, int entriesPerPage)
        {
            List<RFQ> rfqs = new List<RFQ>();
            return rfqs;
        }


        /// <summary>
        /// Given an instance of the RFQ Class, inserts it into the database
        /// Requires that the Customer property of the rfq be set
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="rfq"></param>
        public static void InsertToDB(SqlConnection connection, RFQ rfq)
        {
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO RFQs (EnquiryDate, ProjectName, ReferredBy, CustId) " +
                    "VALUES (@enquiryDate, @projectName, @referredBy, @custId) SELECT SCOPE_IDENTITY()";
                cmd.Parameters.Add("@enquiryDate", System.Data.SqlDbType.Date).Value = rfq.EnquiryDate.Date;
                cmd.Parameters.Add("@projectName", System.Data.SqlDbType.VarChar).Value = rfq.ProjectName;
                cmd.Parameters.Add("@custId", System.Data.SqlDbType.Int).Value = rfq.Customer.Id;
                if (!String.IsNullOrEmpty(rfq.ReferredBy)) { cmd.Parameters.Add("@referredBy", System.Data.SqlDbType.VarChar).Value = rfq.ReferredBy; }
                else { cmd.Parameters.Add("@referredBy", System.Data.SqlDbType.VarChar).Value = DBNull.Value; }

                SqlTransaction transaction = connection.BeginTransaction();
                cmd.Transaction = transaction;
                cmd.Connection = connection;

                try
                { 
                    int RFQId = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.CommandText = "INSERT INTO Items " +
                        "(ItemName, ItemCode, GrossWeight, NetWeight, Status, Priority, Qty, OrderType, RFQId) " +
                        "VALUES (@itemName, @itemCode, @grossWt, @netWt, @status, @priority, @qty, @orderType, @rfqId)";
                    foreach (Item item in rfq.Items)
                    {
                        cmd.Parameters.Add("@itemName", System.Data.SqlDbType.VarChar).Value = item.ItemName;
                        cmd.Parameters.Add("@itemCode", System.Data.SqlDbType.VarChar).Value = item.ItemCode;
                        cmd.Parameters.Add("@grossWt", System.Data.SqlDbType.VarChar).Value = item.GrossWeight;
                        cmd.Parameters.Add("@netWt", System.Data.SqlDbType.VarChar).Value = item.NetWeight;
                        cmd.Parameters.Add("@qty", System.Data.SqlDbType.Int).Value = item.Qty;
                        cmd.Parameters.Add("@status", System.Data.SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@priority", System.Data.SqlDbType.Int).Value = ((int)item.Priority);
                        cmd.Parameters.Add("@orderType", System.Data.SqlDbType.Int).Value = ((int)item.OrderType);
                        cmd.Parameters.Add("@rfqId", System.Data.SqlDbType.Int).Value = RFQId;

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }

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

        }
    }
}

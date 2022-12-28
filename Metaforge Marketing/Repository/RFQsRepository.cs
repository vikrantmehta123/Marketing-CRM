

using Metaforge_Marketing.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Metaforge_Marketing.Repository
{
    public class RFQsRepository
    {
        /// <summary>
        /// TODO: Check with Rahul Fua about the correctness of joins
        /// TODO: Change CustId column to BuyerId
        /// Used for pagination
        /// Fetches the RFQs by their status- fetches RFQs and not the individual items
        /// Will look for inidividual items, and check if there are any items of the given status. 
        /// If there are > 0 such items, the function will add those RFQs to the list
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="itemStatus"></param>
        /// <param name="offsetIndex"></param>
        /// <param name="entriesPerPage"></param>
        /// <returns>A list of RFQs that have at least one item of that status</returns>
        public static IEnumerable<RFQ> FetchRFQs(SqlConnection connection, int itemStatus, int offsetIndex, int entriesPerPage)
        {
            List<RFQ> rfqs = new List<RFQ>();
            using(SqlCommand cmd = new SqlCommand("FetchRFQs", connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@offsetIndex", System.Data.SqlDbType.Int).Value = offsetIndex;
                cmd.Parameters.Add("@entriesPerPage", System.Data.SqlDbType.Int).Value = entriesPerPage;
                cmd.Parameters.Add("@status", System.Data.SqlDbType.Int).Value = itemStatus;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RFQ rfq = new RFQ
                    {
                        Id = Convert.ToInt32(reader["RFQId"]),
                        ProjectName = reader["ProjectName"].ToString(),
                        EnquiryDate = Convert.ToDateTime(reader["EnquiryDate"]),

                        Customer = new Customer
                        {
                            CustomerName = reader["CustomerName"].ToString()
                        }
                    };
                    rfqs.Add(rfq);
                }
                reader.Close();
            }
            return rfqs;
        }

        // Summary:
        //      Fetches the RFQs that are ready for sending quotation (Items are either regretted or customer costing prepared)
        //      Ordered as the Oldest RFQ first
        // Parameters:
        //      Offset Index- How many first rows to skip
        //      Entries Per Page- How Many entries to keep in each page
        public static IEnumerable<RFQ> FetchQuotationReadyRFQs(SqlConnection conn, int offsetIndex, int entriesPerPage)
        {
            List<RFQ> results = new List<RFQ>();
            SqlCommand cmd = new SqlCommand("FetchQuotationReadyRFQs", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@entriesPerPage", System.Data.SqlDbType.Int).Value = entriesPerPage;
            cmd.Parameters.Add("@offsetIndex", System.Data.SqlDbType.Int).Value = offsetIndex;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                RFQ rfq = new RFQ 
                { 
                    Id = Convert.ToInt32(reader["Id"]),
                    ProjectName = reader["ProjectName"].ToString() , 
                    EnquiryDate = Convert.ToDateTime(reader["EnquiryDate"]),
                    Customer = new Customer
                    {
                        CustomerName = reader["CustomerName"].ToString()
                    }
                };
                results.Add(rfq);
            }
            return results;
        }

        // Summary:
        //      Counts the Quotations that are ready for mailing.
        //      Used for pagination
        // Returns:
        //      The count of the RFQs that are quotation ready

        public static int CountQuotationReadyRFQs(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("CountQuotationReadyRFQs", conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }


        /// <summary>
        /// Given the status of an Item, fetches the count of the RFQs where there is at least one item of that status
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="itemStatus"></param>
        /// <returns></returns>
        public static int CountRFQs(SqlConnection conn, int itemStatus)
        {
            using(SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = 
                    "SELECT COUNT(t.RFQId) FROM (" +
		            "SELECT RFQId " + 
		            "FROM Items " +
		            "WHERE Status = @status " +
                    "GROUP BY RFQId " +
		            "HAVING COUNT(*) > 0" +
		            ") t ";
                cmd.Parameters.Add("@status", System.Data.SqlDbType.Int).Value = itemStatus;
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }


        // Summary:
        //      Inserts an RFQ to RFQs table, then inserts the Items in the items table (along with an insert in the ItemHistory table)
        // Parameters:
        //      RFQ- the RFQ that needs to be inserted
        public static void InsertToDB(SqlConnection connection, RFQ rfq)
        {

            SqlCommand ItemHistoryInsertCommand = new SqlCommand("InsertItemHistory", connection);
            ItemHistoryInsertCommand.CommandType = System.Data.CommandType.StoredProcedure;

            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO RFQs (EnquiryDate, ProjectName, ReferredBy, AdminId, BuyerId) " +
                    "VALUES (@enquiryDate, @projectName, @referredBy, @adminId, @buyerId) SELECT SCOPE_IDENTITY()";
                cmd.Parameters.Add("@enquiryDate", System.Data.SqlDbType.Date).Value = rfq.EnquiryDate.Date;
                cmd.Parameters.Add("@projectName", System.Data.SqlDbType.VarChar).Value = rfq.ProjectName;
                cmd.Parameters.Add("@buyerId", System.Data.SqlDbType.Int).Value = rfq.Buyer.Id;
                cmd.Parameters.Add("@adminId", System.Data.SqlDbType.Int).Value = rfq.RFQBroughtBy.Id;
                if (!String.IsNullOrEmpty(rfq.ReferredBy)) { cmd.Parameters.Add("@referredBy", System.Data.SqlDbType.VarChar).Value = rfq.ReferredBy; }
                else { cmd.Parameters.Add("@referredBy", System.Data.SqlDbType.VarChar).Value = DBNull.Value; }

                SqlTransaction transaction = connection.BeginTransaction();
                cmd.Transaction = transaction;
                cmd.Connection = connection;
                ItemHistoryInsertCommand.Transaction = transaction; 

                try
                { 
                    int RFQId = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Parameters.Clear();

                    cmd.CommandText = "INSERT INTO Items " +
                        "(ItemName, ItemCode, GrossWeight, NetWeight, Status, Priority, Qty, OrderType, AdminId, RFQId) " +
                        "VALUES (@itemName, @itemCode, @grossWt, @netWt, @status, @priority, @qty, @orderType, @adminId, @rfqId) SELECT SCOPE_IDENTITY()";
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
                        cmd.Parameters.Add("@adminId", System.Data.SqlDbType.Int).Value = item.QuotationHandledBy.Id;
                        cmd.Parameters.Add("@rfqId", System.Data.SqlDbType.Int).Value = RFQId;

                        int ItemId = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Parameters.Clear();

                        ItemHistoryInsertCommand.Parameters.Add("@itemId", System.Data.SqlDbType.Int).Value = ItemId;
                        ItemHistoryInsertCommand.Parameters.Add("@oldStatus", System.Data.SqlDbType.Int).Value = -1; // As -1 means that there is no item history before this point
                        ItemHistoryInsertCommand.Parameters.Add("@newStatus", System.Data.SqlDbType.Int).Value = 0; // As 0 is status of pending item
                        ItemHistoryInsertCommand.Parameters.Add("@date", System.Data.SqlDbType.Date).Value = rfq.EnquiryDate.Date;
                        ItemHistoryInsertCommand.Parameters.Add("@note", System.Data.SqlDbType.VarChar).Value = "Enquiry for the item was received";

                        ItemHistoryInsertCommand.ExecuteNonQuery();
                        ItemHistoryInsertCommand.Parameters.Clear();
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

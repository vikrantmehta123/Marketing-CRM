

using Metaforge_Marketing.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Metaforge_Marketing.Repository
{
    public class RFQsRepository
    {
        // TODO: Check with Rahul Fua about the correctness of joins
        // TODO: Change CustId column to BuyerId

        // Summary:
        //      Used for pagination
        //      Fetches a list of RFQs based on the status of their items
        //      i.e. If there is at least one item of that status, the RFQ will be in the returned list
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
                    },
                    Buyer = new Buyer
                    {
                        Email = reader["Email"].ToString(),
                        Name = reader["BuyerName"].ToString()
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


        #region Insert Queries

        private static int InsertRFQ(SqlConnection conn, SqlTransaction transaction, RFQ rfq)
        {
            // Add the RFQ Insert Command
            SqlCommand RFQCommand = new SqlCommand("InsertRFQ", conn, transaction)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            RFQCommand.Parameters.Add("@projectName", System.Data.SqlDbType.VarChar).Value = rfq.ProjectName;
            RFQCommand.Parameters.Add("@enquiryDate", System.Data.SqlDbType.Date).Value = rfq.EnquiryDate.Date;
            RFQCommand.Parameters.Add("@buyerId", System.Data.SqlDbType.Int).Value = rfq.Buyer.Id;
            RFQCommand.Parameters.Add("@adminId", System.Data.SqlDbType.Int).Value = rfq.RFQBroughtBy.Id;
            if (!String.IsNullOrEmpty(rfq.ReferredBy)) { RFQCommand.Parameters.Add("@referredBy", System.Data.SqlDbType.VarChar).Value = rfq.ReferredBy; }
            else { RFQCommand.Parameters.Add("@referredBy", System.Data.SqlDbType.VarChar).Value = DBNull.Value; }

            return Convert.ToInt32(RFQCommand.ExecuteScalar());
        }

        private static int InsertItem(SqlConnection conn, SqlTransaction transaction, Item item, RFQ rfq)
        {
            SqlCommand ItemCommand = new SqlCommand("InsertItem", conn, transaction)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            // Add parameters of the command
            ItemCommand.Parameters.Add("@itemName", System.Data.SqlDbType.VarChar).Value = item.ItemName;
            ItemCommand.Parameters.Add("@itemCode", System.Data.SqlDbType.VarChar).Value = item.ItemCode;
            ItemCommand.Parameters.Add("@grossWeight", System.Data.SqlDbType.Decimal).Value = item.GrossWeight;
            ItemCommand.Parameters.Add("@netWeight", System.Data.SqlDbType.Decimal).Value = item.NetWeight;
            ItemCommand.Parameters.Add("@qty", System.Data.SqlDbType.Int).Value = item.Qty;
            ItemCommand.Parameters.Add("@status", System.Data.SqlDbType.Int).Value = 0;
            ItemCommand.Parameters.Add("@priority", System.Data.SqlDbType.Int).Value = ((int)item.Priority);
            ItemCommand.Parameters.Add("@orderType", System.Data.SqlDbType.Int).Value = ((int)item.OrderType);
            ItemCommand.Parameters.Add("@adminId", System.Data.SqlDbType.Int).Value = item.QuotationHandledBy.Id;
            ItemCommand.Parameters.Add("@rfqId", System.Data.SqlDbType.Int).Value = rfq.Id;
            int Id = Convert.ToInt32(ItemCommand.ExecuteScalar());
            ItemCommand.Parameters.Clear();
            return Id;
        }

        private static void InsertItemHistory(SqlConnection conn, SqlTransaction transaction, Item item, RFQ rfq, string note)
        {
            SqlCommand ItemHistoryInsertCommand = new SqlCommand("InsertItemHistory", conn, transaction)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            ItemHistoryInsertCommand.Parameters.Add("@itemId", System.Data.SqlDbType.Int).Value = item.Id;
            ItemHistoryInsertCommand.Parameters.Add("@oldStatus", System.Data.SqlDbType.Int).Value = -1; // As -1 means that there is no item history before this point
            ItemHistoryInsertCommand.Parameters.Add("@newStatus", System.Data.SqlDbType.Int).Value = 0; // As 0 is status of pending item
            ItemHistoryInsertCommand.Parameters.Add("@date", System.Data.SqlDbType.Date).Value = rfq.EnquiryDate.Date;
            ItemHistoryInsertCommand.Parameters.Add("@note", System.Data.SqlDbType.VarChar).Value = note;

            ItemHistoryInsertCommand.ExecuteNonQuery();
            ItemHistoryInsertCommand.Parameters.Clear(); // Clear params for next query
        }


        // Summary:
        //      Inserts an RFQ to RFQs table, then inserts the Items in the items table (along with an insert in the ItemHistory table)
        // Parameters:
        //      RFQ- the RFQ that needs to be inserted
        public static void InsertToDB(SqlConnection connection, RFQ rfq)
        {
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                rfq.Id = InsertRFQ(connection, transaction, rfq);
                foreach (Item item in rfq.Items)
                {
                    item.Id = InsertItem(connection, transaction, item, rfq);
                    InsertItemHistory(connection, transaction, item, rfq, "Enquiry for the item was recd");
                }
                transaction.Commit();
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
            finally { transaction.Dispose(); }
        }

        #endregion Insert Queries
    }
}

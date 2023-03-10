

using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Data;

namespace Metaforge_Marketing.Repository
{
    public class ItemsRepository
    {

        #region Count Functions
        // Summary:
        //      Returns the count of all the items in the database
        public static int CountItems(SqlConnection connection)
        {
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(Id) FROM Items";
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // Summary:
        //      Counts items of a particular status
        public static int CountItems(SqlConnection conn, int status)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(Id) FROM Items WHERE Status = @status";
                cmd.Parameters.Add("@status", SqlDbType.Int).Value = status;
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        #endregion Count Functions
        
        #region Select Queries

        // Summary:
        //      Fetches items of a particular RFQ and of a particular Status
        //      Mostly used in sending Quotations
        // Parameters:
        //      RFQ- the RFQ whose items need to be fetched
        //      status- the Status of the Items (e.g Pending, or Regretted)
        public static IEnumerable<Item> FetchItems(SqlConnection conn, RFQ rfq, ItemStatusEnum status)
        {
            List<Item> items = new List<Item>();
            using(SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Items WHERE RFQId = @rfqId AND Status = @status";
                cmd.Parameters.Add("@status", System.Data.SqlDbType.Int).Value = ((int)status);
                cmd.Parameters.Add("@rfqId", System.Data.SqlDbType.Int).Value = rfq.Id;

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Item item = new Item
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        ItemName = reader["ItemName"].ToString(),
                        Status = (ItemStatusEnum)Convert.ToInt32(reader["Status"]),
                        ItemCode = reader["ItemCode"].ToString(),
                        Qty = Convert.ToInt32(reader["Qty"]),
                        OrderType = (OrderTypeEnum)(Convert.ToInt16(reader["OrderType"])),
                        Priority = (PriorityEnum)(Convert.ToInt16(reader["Priority"])),
                        GrossWeight = (float)Convert.ToDecimal(reader["GrossWeight"]),
                        NetWeight = (float)Convert.ToDecimal(reader["NetWeight"])
                    };
                    items.Add(item);
                }
                reader.Close();
            }

            return items;
        }

        public static IEnumerable<Item> FetchItems(SqlConnection conn, int offsetIndex, int entriesPerPage, int status)
        {
            List<Item> items = new List<Item>();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = 
                "SELECT * FROM Items " +
                "JOIN RFQs ON RFQs.Id = Items.RFQId " +
                "WHERE Status = @status " +
                "ORDER BY EnquiryDate DESC " +
                "OFFSET @offsetIndex ROWS FETCH NEXT @entriesPerPage ROWS ONLY";
            cmd.Parameters.Add("@offsetIndex", SqlDbType.Int).Value =offsetIndex;
            cmd.Parameters.Add("@entriesPerPage", SqlDbType.Int).Value=entriesPerPage;
            cmd.Parameters.Add("@status", SqlDbType.Int).Value = status;

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Item item = new Item
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    ItemName = reader["ItemName"].ToString(),
                    ItemCode = reader["ItemCode"].ToString(),
                    GrossWeight = (float)Convert.ToDecimal(reader["GrossWeight"]),
                    NetWeight = (float)Convert.ToDecimal(reader["NetWeight"]), 
                    Qty = Convert.ToInt32(reader["Qty"]),
                    OrderType = (OrderTypeEnum)Convert.ToInt16(reader["OrderType"]),
                    Priority = (PriorityEnum)(Convert.ToInt16(reader["Priority"]))
                };

                items.Add(item);
            }
            reader.Close();
            return items;
        }

        public static IEnumerable<Item> FetchItems(SqlConnection conn, RFQ rfq)
        {
            List<Item> items = new List<Item>();
            using(SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Items WHERE RFQId = @rfqId";
                cmd.Parameters.Add("@rfqId", System.Data.SqlDbType.Int).Value = rfq.Id;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Item item = new Item
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        ItemName = reader["ItemName"].ToString(),
                        Status = (ItemStatusEnum)Convert.ToInt32(reader["Status"]),
                        ItemCode = reader["ItemCode"].ToString(),
                        Qty = Convert.ToInt32(reader["Qty"]),
                        OrderType = (OrderTypeEnum)(Convert.ToInt16(reader["OrderType"])),
                        Priority = (PriorityEnum)(Convert.ToInt16(reader["Priority"]))
                    };
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }


        // Summary:
        //      Fetches items of all status for pagination
        public static IEnumerable<Item> FetchItems(SqlConnection connection, int offsetIndex, int entriesPerPage)
        {
            List<Item> items = new List<Item>();
            using(SqlCommand cmd = new SqlCommand("FetchItems",connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@offsetIndex", System.Data.SqlDbType.Int).Value = offsetIndex;
                cmd.Parameters.Add("@entriesPerPage", System.Data.SqlDbType.Int).Value =entriesPerPage;

                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Item item = new Item
                        {
                            RFQ = new RFQ(),
                            Customer = new Customer(),

                            Id = Convert.ToInt32(reader["ItemId"]),
                            ItemName = reader["ItemName"].ToString(),
                            Status = (ItemStatusEnum)Convert.ToInt32(reader["Status"]),
                            ItemCode = reader["ItemCode"].ToString(),
                            Qty = Convert.ToInt32(reader["Qty"]),
                            OrderType = (OrderTypeEnum)(Convert.ToInt16(reader["OrderType"])),
                            Priority = (PriorityEnum)(Convert.ToInt16(reader["Priority"])),
                            GrossWeight = (float)Convert.ToDecimal(reader["GrossWeight"])
                        };
                        item.Customer.CustomerName = reader["CustomerName"].ToString();
                        item.Customer.City = reader["City"].ToString();
                        item.RFQ.Id = Convert.ToInt32(reader["RFQId"]);
                        item.RFQ.ProjectName = reader["ProjectName"].ToString();

                        items.Add(item);
                    }
                    reader.Close();
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }
            return items;
        }

        // Summary:
        //      Search function for the Database. Searches based on the Item's name
        public static IEnumerable<Item> FetchItems(SqlConnection connection, string searchText)
        {
            List<Item> items = new List<Item>();
            using(SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT Items.ItemName, Items.ItemCode, Items.Id AS ItemId, Items.Status, Items.Priority, Items.Qty, Items.OrderType, " +
                    "RFQs.Id AS RFQId, RFQs.ProjectName, Customers.CustomerName, Customers.City " +
                    "FROM Items " +
                    "LEFT JOIN RFQs ON RFQs.Id = Items.RFQId " +
                    "LEFT JOIN Customers ON Customers.Id = RFQs.CustId " +
                    "WHERE Items.ItemName LIKE @itemName";

                cmd.Parameters.Add("@itemName", SqlDbType.VarChar).Value = $"%{searchText}%";

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Item item = new Item
                    {
                        RFQ = new RFQ(),
                        Customer = new Customer(),

                        Id = Convert.ToInt32(reader["ItemId"]),
                        ItemName = reader["ItemName"].ToString(),
                        Status = (ItemStatusEnum)Convert.ToInt32(reader["Status"]),
                        ItemCode = reader["ItemCode"].ToString(),
                        Qty = Convert.ToInt32(reader["Qty"]),
                        OrderType = (OrderTypeEnum)(Convert.ToInt16(reader["OrderType"])),
                        Priority = (PriorityEnum)(Convert.ToInt16(reader["Priority"]))
                    };
                    item.Customer.CustomerName = reader["CustomerName"].ToString();
                    item.Customer.City = reader["City"].ToString();
                    item.RFQ.Id = Convert.ToInt32(reader["RFQId"]);
                    item.RFQ.ProjectName = reader["ProjectName"].ToString();

                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }


        public static DataTable FetchItemsIntoDataTable(SqlConnection conn, int offsetIndex, int entriesPerPage)
        {
            DataTable table = new DataTable();
            SqlCommand cmd = new SqlCommand("FetchItems", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@offsetIndex", SqlDbType.Int).Value = offsetIndex;
            cmd.Parameters.Add("@entriesPerPage", SqlDbType.Int).Value = entriesPerPage;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(table);
            return table;
        }

        #endregion Select Queries

        #region Update Queries

        public static void UpdateDB(SqlConnection conn, DataTable table)
        {
            SqlCommand cmd = new SqlCommand("UpdateItem", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@id", SqlDbType.Int, 32, "ItemId");
            cmd.Parameters.Add("@itemName", SqlDbType.VarChar, 32, "ItemName");
            cmd.Parameters.Add("@itemCode", SqlDbType.VarChar, 32, "ItemCode");
            cmd.Parameters.Add("@priority", SqlDbType.Int, 32, "Priority");
            cmd.Parameters.Add("@qty", SqlDbType.Int, 32, "Qty");

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.UpdateCommand = cmd;

            adapter.Update(table);
        }
        #endregion Update Queries



    }
}

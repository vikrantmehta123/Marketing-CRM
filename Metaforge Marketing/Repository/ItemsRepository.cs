

using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace Metaforge_Marketing.Repository
{
    public class ItemsRepository
    {
        #region Select Queries

        /// <summary>
        /// Returns the count of all items present in database
        /// Requires an open connection
        /// Used in pagination
        /// </summary>
        /// <param name="connection"></param>
        /// <returns>Count of items in database</returns>
        public static int CountItems(SqlConnection connection)
        {
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(Id) FROM Items";
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
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
                    item.InitIndicatorVariables();
                    items.Add(item);
                }
                reader.Close();
            }
            return items;
        }


        /// <summary>
        /// Fetches a list of Items, along with its RFQ and the customer
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="offsetIndex"></param>
        /// <param name="entriesPerPage"></param>
        /// <returns></returns>
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
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }
            return items;
        }

        /// <summary>
        /// Given a search text of the item's Name, fetches the results that are like the name
        /// Used in pagination
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="searchText"></param>
        /// <returns>List of Items that are match to the Name given</returns>
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
                    "WHERE Items.ItemName LIKE @itemId";

                cmd.Parameters.Add("@itemId", System.Data.SqlDbType.VarChar).Value = $"%{searchText}%";

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

        #endregion Select Queries

        #region Insert Queries
        public static void InsertToDB(SqlConnection connection, Item item)
        {

        }
        #endregion Insert Queries
    }
}

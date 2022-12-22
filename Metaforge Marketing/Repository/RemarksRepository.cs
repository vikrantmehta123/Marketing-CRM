

using Metaforge_Marketing.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Metaforge_Marketing.Repository
{
    public class RemarksRepository
    {

        /// <summary>
        /// Inserts a remark in the Remarks table
        /// Requires that the Customer Property of the Remark be set ( Customer.Id is reqd)
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="remark"></param>
        public static void InsertToDB(SqlConnection conn, Remark remark)
        {
            SqlCommand cmd = new SqlCommand("InsertRemark", conn);
            cmd.Parameters.Add("@custId", System.Data.SqlDbType.Int).Value = remark.Customer.Id ;
            cmd.Parameters.Add("@date", System.Data.SqlDbType.Date).Value = remark.EventDate.Date;
            cmd.Parameters.Add("@remark", System.Data.SqlDbType.VarChar).Value = remark.Text;

            cmd.ExecuteNonQuery();
        }


        /// <summary>
        /// Given an instance of the customer, fetches all the remarks of that customer
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cust"></param>
        /// <returns>List of remarks</returns>
        public static IEnumerable<Remark> FetchRemarks(SqlConnection conn, Customer cust)
        {
            List<Remark> list = new List<Remark>();
            SqlCommand cmd = new SqlCommand("FetchRemarks", conn);
            cmd.Parameters.Add("@custId", System.Data.SqlDbType.Int).Value = cust.Id;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Remark remark = new Remark();
                remark.Text = reader["Remark"].ToString();
                remark.EventDate = Convert.ToDateTime(reader["RemarkDate"]);
                remark.Id = Convert.ToInt32(reader["Id"]);
                list.Add(remark);
            }
            reader.Close();
            return list;
        }
    }
}

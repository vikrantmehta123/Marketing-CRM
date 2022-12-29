

using Metaforge_Marketing.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Metaforge_Marketing.Repository
{
    public class RemarksRepository
    {

        // Summary:
        //      Inserts a Remark in the database
        //      Requires that the Customer property of the Remark be set
        // Parameters:
        //      SqlConnection- Open Connection
        //      Remark- the remark that needs to be added
        public static void InsertToDB(SqlConnection conn, Remark remark)
        {
            SqlCommand cmd = new SqlCommand("InsertRemark", conn)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            cmd.Parameters.Add("@remark", System.Data.SqlDbType.VarChar).Value = remark.Note.Trim();
            cmd.Parameters.Add("@custId", System.Data.SqlDbType.Int).Value = remark.Customer.Id ;
            cmd.Parameters.Add("@date", System.Data.SqlDbType.Date).Value = remark.EventDate.Date;

            cmd.ExecuteNonQuery();
        }


        // Summary:
        //      Fetches all the records of the Customer, that are present in the database
        //      Used in creating Customer History report
        // Parameters:
        //      SqlConnection - an open connection
        //      Customer- Whose remarks need to be fetched
        public static IEnumerable<Remark> FetchRemarks(SqlConnection conn, Customer cust)
        {
            List<Remark> list = new List<Remark>();
            SqlCommand cmd = new SqlCommand("FetchRemarks", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@custId", System.Data.SqlDbType.Int).Value = cust.Id;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Remark remark = new Remark
                {
                    Note = reader["Remark"].ToString(),
                    EventDate = Convert.ToDateTime(reader["RemarkDate"]),
                    Id = Convert.ToInt32(reader["Id"])
                };
                list.Add(remark);
            }
            reader.Close();
            return list;
        }
    }
}

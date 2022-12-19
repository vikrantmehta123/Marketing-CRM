

using Microsoft.Data.SqlClient;
using System;

namespace Metaforge_Marketing.HelperClasses
{
    public class SQLWrapper<T>
    {
        private static string conn_string { get; } = Metaforge_Marketing.Properties.Settings.Default.conn_string;

        /// <summary>
        /// Provides a wrapper to insert an object into the database
        /// </summary>
        /// <param name="action"></param>
        /// <param name="objToInsert"></param>
        public static void InsertWrapper(Action<SqlConnection, T> action, T objToInsert)
        {
            using (SqlConnection connection = new SqlConnection(conn_string))
            {
                connection.Open();
                action(connection, objToInsert);
                connection.Close();
            }
        }
    }
}

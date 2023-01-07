

using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Metaforge_Marketing.HelperClasses
{
    public class SQLWrapper<T>
    {
        #region Fields
        private static string conn_string = Metaforge_Marketing.Properties.Settings.Default.conn_string;
        #endregion Fields


        #region Insert Wrappers

        // Provides an wrapper for INSERT INTO type of queries
        public static void InsertWrapper(Action<SqlConnection, T> action, T objToInsert)
        {
            using (SqlConnection connection = new SqlConnection(conn_string))
            {
                connection.Open();
                action(connection, objToInsert);
                connection.Close();
            }
        }

        #endregion Insert Wrappers

        #region Search Wrappers
        /// <summary>
        /// Provides a wrapper for the Search functions of the database
        /// Used in pagination
        /// </summary>
        /// <param name="searchFunction"></param>
        /// <param name="searchText"></param>
        /// <returns>IEnumerable of the matching results</returns>
        public static IEnumerable<T> SearchWrapper(Func<SqlConnection, string, IEnumerable<T>> searchFunction, string searchText)
        {
            IEnumerable<T> result;
            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                result = searchFunction(conn, searchText);
                conn.Close();
            }
            return result;
        }
        #endregion Search Wrappers

        #region Fetch Wrappers

        // Summary:
        //      Provides wrapper for queries where foreign key is reqd
        public static IEnumerable<T> FetchWrapper<S>(Func<SqlConnection, S, IEnumerable<T>> fetchFunction, S obj)
        {
            IEnumerable<T> result;
            using(SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                result = fetchFunction(conn, obj);
                conn.Close();
            }
            return result;
        }

        // Summary:
        //      Provides a wrapper for SELECT * type of queries
        public static IEnumerable<T> FetchWrapper(Func<SqlConnection, IEnumerable<T>> fetchFunction)
        {
            IEnumerable<T> result;
            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                result = fetchFunction(conn);
                conn.Close();
            }
            return result;
        }

        // Summary:
        //      Provides a wrapper for the SELECT-OFFSET-FETCH type of queries
        public static IEnumerable<T> FetchWrapper(Func<SqlConnection, int, int, IEnumerable<T>> fetchFunction, int offsetIndex, int entriesPerPage)
        {
            IEnumerable<T> result;
            using(SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                result = fetchFunction(conn, offsetIndex, entriesPerPage);
                conn.Close();
            }
            return result;
        }

        #endregion Fetch Wrappers

        #region Count Wrappers
        public static int CountWrapper(Func<SqlConnection, int> countFunction)
        {
            int count = 0;
            using(SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                count = countFunction(conn);    
                conn.Close();
            }
            return count;
        }

        public static int CountWrapper<S>(Func<SqlConnection, S,  int> countFunction, S obj)
        {
            int count = 0;
            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                count = countFunction(conn, obj);
                conn.Close();
            }
            return count;
        }
        #endregion Count Wrappers
    }
}



using Microsoft.Data.SqlClient;
using System;

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
        #endregion Select Queries
    }
}

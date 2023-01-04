using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Metaforge_Marketing.Repository
{
    public class TestRepository
    {
        #region Select Queries

        // Summary:
        //      Fetches a Conversion Quotation record into a Datatable based on the Category
        //      Used in inserting/ updating costing
        public static DataTable FetchConvCosting(SqlConnection conn, Item item, CostingCategoryEnum category)
        {
            DataTable table = new DataTable();
            SqlCommand cmd = new SqlCommand("SELECT ConversionCostings.*, Operations.Name FROM ConversionCostings INNER JOIN Operations ON Operations.Id = OperationId WHERE ItemId = @itemId AND WhoseCosting = @whoseCosting ", conn);
            cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = item.Id;
            cmd.Parameters.Add("@whoseCosting", SqlDbType.Int).Value = ((int)category);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(table);
            return table;
        }

        #endregion Select Queries
    }
}

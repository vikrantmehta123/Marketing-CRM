

using Metaforge_Marketing.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace Metaforge_Marketing.Repository
{
    public class RFQsRepository
    {
        public static IEnumerable<RFQ> FetchRFQs(SqlConnection connection, int rfqStatus)
        {
            List<RFQ> rfqs = new List<RFQ>();
            return rfqs;
        }
    }
}

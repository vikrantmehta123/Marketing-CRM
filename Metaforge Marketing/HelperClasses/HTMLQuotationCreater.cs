

using Metaforge_Marketing.Models;
using System.Collections.Generic;

namespace Metaforge_Marketing.HelperClasses
{
    public class HTMLQuotationCreater
    {
        /// public void CreateQuotationWithoutBreakup(IEnumerable<Costing> costings)
        /// {
        ///     string HTMLTable = "<table>";
        ///     HTMLTable = WriteHeaders(costings, HTMLtable);
        /// 
        ///     HTMLTable += </table>
        /// }
        /// 

        public string WriteHeaders(IEnumerable<Costing> costings, ref string htmlTable)
        {
            string header = string.Empty;
            string rowStartTag = "<tr>";
            string emptyTopCornerCell = "<td> </td>";
            string rowEndTag = "</tr>";
            header += rowStartTag;
            header += emptyTopCornerCell;
            foreach (Costing costing in costings)
            {
                string cell = $"<th>{costing.Item.ItemName}</th>";
                header += cell;
            }
            header += rowEndTag;
            htmlTable += header;
            return htmlTable;
        }

        public string WriteCC(IEnumerable<Costing> costings, ref string htmlTable)
        {
            string row = string.Empty;
            string rowStartTag = "<tr>";
            string rowEndTag = "</tr>";

            string rowHeader = "<th> Total Conversion Cost </th>";

            row += rowStartTag;
            row += rowHeader;
            foreach (Costing costing in costings)
            {
                //row += $"<td> {costing.CCPerPiece} </td>";
            }
            row += rowEndTag;

            htmlTable += row;
            return htmlTable;
        }
    }
}

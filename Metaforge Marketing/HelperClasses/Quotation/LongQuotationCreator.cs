using Metaforge_Marketing.Models;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaforge_Marketing.HelperClasses.Quotation
{
    public class LongQuotationCreator : PDFQuotationCreator
    {
        private static int AddRMHeaders(ref Table table, int index)
        {
            List<string> headers = new List<string> { "Material", "Gross Weight", "RM Rate", "RM Cost" };
            int looper = 0;
            int i;
            for (i = index; i <= index + headers.Count; i++)
            {
                table.Cell(i, 1).Range.Text = headers[looper];
                looper++;
            }
            return i;
        }
        private static int AddConversionCostingHeaders(ref Table table, ConversionCosting convCosting, int index)
        {
            int looper = 0;
            int i;
            for (i = index; i <= index + convCosting.Operations.Count; i++)
            {
                table.Cell(i, 1).Range.Text = convCosting.Operations[looper].OperationName;
                looper++;
            }
            return i;
        }
        private static int AddSummaryRowHeaders(ref Table table, int index)
        {
            List<string> headers = new List<string> { "Total MFG Cost", "Add 15% Profit", "Total Price" };
            int looper = 0;
            int i;
            for (i = index; i <= index + headers.Count; i++)
            {
                Cell cell = table.Cell(i, 1);
                cell.Range.Text = headers[looper];
                if (looper == headers.Count - 1)
                {
                    AddHeaderFormatting(ref cell);
                }
                looper++;
            }
            return i;
        }
        private static int AddItemHeaders(ref Table table, int index)
        {
            List<string> headers = new List<string> { "Part Name", "Part No" };
            int looper = 0;
            int i;
            for (i = index; i <= index + headers.Count; i++)
            {
                Cell cell = table.Cell(i, 1);
                cell.Range.Text = headers[looper];
                AddHeaderFormatting(ref cell);
                looper++;
            }
            return i;
        }
        private static Table AddWithBreakUpQuotationRowHeaders(ref Table table, ConversionCosting convCosting)
        {
            int currRow = 1;
            currRow = AddItemHeaders(ref table, currRow);
            currRow = AddRMHeaders(ref table, currRow);
            currRow = AddConversionCostingHeaders(ref table, convCosting, currRow);
            currRow = AddSummaryRowHeaders(ref table, currRow);
            return table;
        }
        private static Document AddWithBreakUpQuotationTable(ref Document doc, Costing costing)
        {
            Table QuotationTable;
            Range range = doc.Bookmarks.get_Item(ref oEndOfDoc).Range;

            int numOfColumns = 2; // One for headers, and one for the values
            int numOfRows = 2 + 4 + costing.ConvCosting.Operations.Count + 3 + 2; // ItemName/Code, 4 RM Headers, Operations, 3 Summary headers, 2 blank rows in between = 11 + operations

            QuotationTable = doc.Tables.Add(range, numOfRows, numOfColumns, ref oMissing, ref oMissing); // Add the Quotation Table to the doc

            QuotationTable.Borders.Enable = 1; // Add Borders to the table
            return doc;
        }

        private static Table AddWithBreakUpQuotationCostingTotals(ref Table table, Costing costing)
        {
            return table;
        }
    }
}

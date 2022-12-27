using Metaforge_Marketing.Models;
using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Metaforge_Marketing.HelperClasses.Quotation
{
    public class ShortQuotationCreator : PDFQuotationCreator
    {
        // Summary:
        //      Defines methods to create and send a quotation in its short format

        public static string CreateQuotation(IEnumerable<Costing> costings)
        {
            Application app = CreateApplication(); // Open Application
            Document quote = AddDocument(ref app); // Add Document to that app
            AddDocumentHeader(ref quote, "Quotation"); // Add Header to the do
            quote = AddShortQuotationTable(ref quote, costings); // Add the table of the quotes
            string path;
            try
            {
                path = SaveQuotation(ref quote);
                return path;
            }
            finally
            {
                object saveOption = WdSaveOptions.wdDoNotSaveChanges;
                object originalFormat = WdOriginalFormat.wdOriginalDocumentFormat;
                object routeDocument = false;
                quote.Close(ref saveOption, ref originalFormat, ref routeDocument);
                app.Quit();
            }
            
        }

        private static Document AddShortQuotationTable(ref Document doc, IEnumerable<Costing> costings)
        {
            // TODO: Depending on the number of costings, create additional tables
            //       For example, if the number of costings is > 7, then create two tables, with the first table having 5 costings, and the tables below having the remaining entries

            Table QuotationTable;
            Range range = doc.Bookmarks.get_Item(ref oEndOfDoc).Range;

            int numOfRows = 6; // ItemName, ItemCode, TotalRMCost, TotalCC, Add Profit, Total
            int numOfColumns = 1 + costings.Count(); // One for the Row Headers, and others for the items

            QuotationTable = doc.Tables.Add(range, numOfRows, numOfColumns, ref oMissing, ref oMissing); // Add the Quotation Table to the doc
            QuotationTable = AddShortQuotationRowHeaders(ref QuotationTable); // Add the row headers to the table (See row headers in line (42)
            QuotationTable = AddShortQuotationCostingTotals(ref QuotationTable, costings.ToList()); // Add the costings table
            QuotationTable.Borders.Enable = 1; // Add Borders to the table

            return doc;
        }
        /// <summary>
        /// Hardcodes the row headers to the table
        /// </summary>
        /// <param name="table"></param>
        /// <returns> The table which was given as input</returns>
        /// <exception cref="Exception"></exception>
        private static Table AddShortQuotationRowHeaders(ref Table table)
        {
            List<string> headers = new List<string> { "Item Name", "Item Code", "Total RM Cost", "Total Conversion Cost", "Add 15% Profit", "Total Cost Per Piece" };
            if (table.Rows.Count != 6) { throw new Exception("Row count is not 6!"); }

            for (int i = 1; i <= table.Rows.Count; i++)
            {
                Cell cell = table.Cell(i, 1);

                // Add Formatting
                cell.Range.Font.Bold = 1;
                cell.Range.Font.Name = "verdana";
                cell.Range.Font.Size = 8;
                cell.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                if (i == 1 || i == 2)
                {
                    cell.Shading.BackgroundPatternColor = WdColor.wdColorGray25; // Color the background if the rows are header rows
                }

                // Add Header
                table.Cell(i, 1).Range.Text = headers[i - 1];
            }
            return table;
        }

        /// <summary>
        /// Given a table and a list of costings, adds the costs for the short format in the table
        /// </summary>
        /// <param name="table"></param>
        /// <param name="costings"></param>
        /// <returns>Returns the table which was updated</returns>
        private static Table AddShortQuotationCostingTotals(ref Table table, List<Costing> costings)
        {
            for (int i = 2; i <= table.Columns.Count; i++)
            {
                int index = i - 2;
                Cell ItemNameHeader = table.Cell(1, i);
                Cell ItemCodeHeader = table.Cell(2, i);

                // Add formatting to the first two header rows
                AddHeaderFormatting(ref ItemCodeHeader);
                AddHeaderFormatting(ref ItemNameHeader);

                // Fill table
                ItemNameHeader.Range.Text = costings[index].Item.ItemName;
                ItemCodeHeader.Range.Text = costings[index].Item.ItemCode;
                table.Cell(3, i).Range.Text = costings[index].RMCosting.CostPerPiece.ToString();
                table.Cell(4, i).Range.Text = costings[index].ConvCosting.TotalCostPerPiece.ToString();
                table.Cell(5, i).Range.Text = costings[index].AddProfit().ToString();
                table.Cell(6, i).Range.Text = costings[index].ComputeTotalCost().ToString();
            }
            return table;
        }

    }
}

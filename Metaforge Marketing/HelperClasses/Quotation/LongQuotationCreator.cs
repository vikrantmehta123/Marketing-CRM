using Metaforge_Marketing.Models;
using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.Reflection;

namespace Metaforge_Marketing.HelperClasses.Quotation
{
    public class LongQuotationCreator : PDFQuotationCreator
    {
        #region Helper Methods
        // Summary:
        //      Inserts a table in the document
        private static Table AddTableToDocument(ref Document doc, Costing costing)
        {
            Table QuotationTable;
            Range range = doc.Bookmarks.get_Item(ref oEndOfDoc).Range;

            int numOfColumns = 2; // One for headers, and one for the values
            int numOfRows = 2 + 4 + costing.ConvCosting.Operations.Count + 3 + 2; // ItemName/Code, 4 RM Headers, Operations, 3 Summary headers, 2 blank rows in between = 11 + operations

            QuotationTable = doc.Tables.Add(range, numOfRows, numOfColumns, ref oMissing, ref oMissing); // Add the Quotation Table to the doc

            QuotationTable.Borders.Enable = 1; // Add Borders to the table
            return QuotationTable;
        }

        #endregion Helper Methods

        #region Main Methods

        // Summary:
        //      Fills the table with the headers and values from the costing
        private static Table AddQuotationTable(ref Table table, Costing costing)
        {
            int currRow = 1;

            AddItemHeaders(ref table, currRow); 
            currRow = AddItemValues(ref table, costing, currRow); 

            AddRMHeaders(ref table, currRow);
            currRow = AddRMValues(ref table, costing, currRow);
            currRow = AddBlankRow(ref table, currRow);

            AddConversionCostingHeaders(ref table, costing.ConvCosting, currRow);
            currRow = AddCCValues(ref table, costing, currRow);
            currRow = AddBlankRow(ref table, currRow);

            AddSummaryRowHeaders(ref table, currRow);
            AddSummaryValues(ref table, costing, currRow);
            return table;
        }

        // Summary:
        //      Creates a quotation document from the given list of costings
        // Returns:
        //      string- The filepath of the document saved
        public static string CreateQuotation(List<Costing> costings)
        {
            Application app = CreateApplication();
            Document doc = AddDocument(ref app);
            AddDocumentHeader(ref doc, "K21 Quotation");
            for(int i = 0; i < costings.Count; i++)
            {
                Table QuoteTable = AddTableToDocument(ref doc, costings[i]);

                QuoteTable = AddQuotationTable(ref QuoteTable, costings[i]);

                if (i < costings.Count - 1) {  doc = InsertPageBreak(ref doc); } // Insert a page break for all items except the last one
            }
            string path = "";
            try
            {
                path= SaveQuotation(ref doc);
            }
            catch(System.Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            } 
            return path;
        }
        #endregion Main Methods

        #region Row Headers

        // Summary:
        //      Adds the RM Headers to the document. See the list within the function to know what headers are added
        // Returns:
        //      The index of the next row(which is blank)
        // Parameters:
        //      The row number (1-indexed) from which to insert the headers
        private static int AddRMHeaders(ref Table table, int index)
        {
            List<string> headers = new List<string> { "Material", "Gross Weight", "RM Rate", "RM Cost" };
            int looper = 0;
            int i;
            for (i = index; i < index + headers.Count; i++)
            {
                table.Cell(i, 1).Range.Text = headers[looper];
                looper++;
            }
            return i;
        }

        // Summary:
        //      Adds the Conversion Costing headers- OperationNames are considered as headers
        private static int AddConversionCostingHeaders(ref Table table, ConversionCosting convCosting, int index)
        {
            int looper = 0;
            int i;
            for (i = index; i < index + convCosting.Operations.Count; i++)
            {
                table.Cell(i, 1).Range.Text = convCosting.Operations[looper].OperationName;
                looper++;
            }
            return i;
        }

        // Summary:
        //      Adds the summary row headers-> See the list inside to know which headers are added
        private static int AddSummaryRowHeaders(ref Table table, int index)
        {
            List<string> headers = new List<string> { "Total MFG Cost", "Add 15% Profit", "Total Price" };
            int looper = 0;
            int i;
            for (i = index; i < index + headers.Count; i++)
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

        // Summary:
        //      Adds the Item headers- Item Name, Item Code
        private static int AddItemHeaders(ref Table table, int index)
        {
            List<string> headers = new List<string> { "Part Name", "Part No" };
            int looper = 0;
            int i;
            for (i = index; i < index + headers.Count; i++)
            {
                Cell cell = table.Cell(i, 1);
                cell.Range.Text = headers[looper];
                AddHeaderFormatting(ref cell);
                looper++;
            }
            return i;
        }
        #endregion Row Headers

        #region Adding Costing Values

        private static int AddRMValues(ref Table table, Costing costing, int index)
        {
            int currRow = index;
            table.Cell(currRow, 2).Range.Text = costing.RMCosting.RMConsidered.ToString(); currRow++;
            table.Cell(currRow, 2).Range.Text = costing.Item.GrossWeight.ToString(); currRow++;
            table.Cell(currRow, 2).Range.Text = costing.RMCosting.RMRate.ToString(); currRow++;
            table.Cell(currRow, 2).Range.Text = costing.RMCosting.CostPerPiece.ToString(); currRow++;
            return currRow;
        }

        private static int AddCCValues(ref Table table, Costing costing, int rowIndex)
        {
            int looper = 0;
            int i;
            for (i = rowIndex; i < rowIndex + costing.ConvCosting.Operations.Count; i++)
            {
                table.Cell(i, 2).Range.Text = costing.ConvCosting.Operations[looper].CostPerPiece.ToString();
                looper++;
            }
            return rowIndex + looper;
        }

        private static int AddSummaryValues(ref Table table, Costing costing, int rowIndex)
        {
            int currRow = rowIndex;
            table.Cell(currRow, 2).Range.Text = (costing.RMCosting.CostPerPiece + costing.ConvCosting.TotalCostPerPiece).ToString() ; currRow++;
            table.Cell(currRow, 2).Range.Text = costing.AddProfit().ToString(); currRow++;

            Cell totalCell = table.Cell(currRow, 2);
            totalCell.Range.Text = costing.ComputeTotalCost().ToString();
            AddHeaderFormatting(ref totalCell);
            currRow++;
            return currRow;
        }

        private static int AddItemValues(ref Table table, Costing costing, int rowIndex)
        {
            int currRow = rowIndex;

            // Add the first- Item name
            Cell cell1 = table.Cell(currRow, 2);
            cell1.Range.Text = costing.Item.ItemName;
            AddHeaderFormatting(ref cell1); currRow++;

            // Add the second row- Item no
            Cell cell2 = table.Cell(currRow, 2);
            cell2.Range.Text = costing.Item.ItemCode;
            AddHeaderFormatting(ref cell2); currRow++;

            return currRow;
        }
        #endregion Adding Costing Values
    }
}

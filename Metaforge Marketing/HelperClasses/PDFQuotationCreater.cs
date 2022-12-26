using System;
using System.Collections.Generic;
using System.Linq;
using Metaforge_Marketing.Models;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;

namespace Metaforge_Marketing.HelperClasses
{
    public class PDFQuotationCreater
    {
        #region Fields
        private static object oMissing = System.Reflection.Missing.Value;
        private static object oEndOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */
        #endregion Fields

        #region Common Methods (Shared By short & long quotation)
        private static Application CreateApplication()
        {
            // TODO: Set Visible = false after testing
            Application app = new Application
            {
                Visible = true,
                ShowAnimation = false
            };
            return app;
        }

        // Summary:
        //      Adds a document to the given app
        //
        // Returns:
        //      The Document that was added
        private static Document AddDocument(ref Application app)
        {
            Document doc = app.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            return doc;
        }
        
        // Summary:
        //      Adds the header to the given document
        private static void AddDocumentHeader(ref Document doc, string header)
        {
            foreach (Section section in doc.Sections)
            {
                //Get the header range and add the header details.
                Range headerRange = section.Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                headerRange.Fields.Add(headerRange, WdFieldType.wdFieldPage);
                headerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                headerRange.Font.ColorIndex = WdColorIndex.wdBlack;
                headerRange.Font.Size = 20;
                headerRange.Text = $"{header}";
            }
        }

        // Summary:
        //      Saves the given document after prompting
        //  Exceptions:
        //      Throws an exception if the user doesn't save
        // Returns:
        //      The path of the saved file
        private static string SaveQuotation(ref Document document)
        {
            //Save the document
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PDF |*.pdf";
            bool res = (bool)dialog.ShowDialog();
            if (res)
            {
                string path = dialog.FileName.ToString();
                document.ExportAsFixedFormat(path, WdExportFormat.wdExportFormatPDF);
                return path;
            }
            else
            {
                throw new Exception("Please Save the file");
            }
        }

        private static Table AddBlankRow(ref Table table, int index)
        {
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                table.Cell(index, i).Range.Text = "";
            }
            return table;
        }

        private static void AddHeaderFormatting(ref Cell cell)
        {
            cell.Range.Font.Bold = 1; // Set font weight
            cell.Range.Font.Name = "verdana"; // Set font
            cell.Range.Font.Size = 8;         // Set font size  
            cell.Shading.BackgroundPatternColor = WdColor.wdColorGray25;
            cell.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter; //Center alignment for the Header cells
            cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter; //Center alignment for the Header cells
        }
        #endregion Common Methods

        #region Short Quotation Methods

        public static void SendShortQuotation(IEnumerable<Costing> costings, Buyer recipient)
        {
            Application app = CreateApplication(); // Open Application
            Document quote = AddDocument(ref app); // Add Document to that app
            AddDocumentHeader(ref quote, "Quotation"); // Add Header to the do
            quote = AddShortQuotationTable(ref quote, costings); // Add the table of the quotes
            string path;
            try
            {
                path = SaveQuotation(ref quote);
                Email email = new Email();
                email.MailMessage.To.Add(recipient.Email);
                email.MailMessage.Attachments.Add(new System.Net.Mail.Attachment(path));

                // TODO: Add the body to the email
                email.MailMessage.Body = "";
                email.Send();
            } 
            catch(Exception ex)
            {
                
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
                AddHeaderFormatting(ItemCodeHeader);
                AddHeaderFormatting(ItemNameHeader);

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

        #endregion Short Quotation Methods

        #region With Break Up Quotation Methods
        private static int AddRMHeaders(ref Table table, int index)
        {
            List<string> headers = new List<string> { "Material", "Gross Weight", "RM Rate", "RM Cost" };
            int looper = 0;
            int i = index;
            for (int i; i <= index + headers.Count; i++)
            {
                table.Cell(i, 1).Range.Text = headers[looper];
                looper++;
            }
            return i;
        }
        private static int AddConversionCostingHeaders(ref Table table, ConversionCosting convCosting, int index)
        {
            int looper = 0;
            int i = index;
            for (int i; i <= index + convCosting.Operations.Count; i++)
            {
                table.Cell(i, 1).Range.Text = convCosting.Operations[looper].OperationName;
                looper++;
            }
        }
        private static int AddSummaryRowHeaders(ref Table table, int index)
        {
            List<string> headers = new List<string> { "Total MFG Cost", "Add 15% Profit", "Total Price" };
            int looper = 0;
            int i = index;
            for (int i; i <= index + headers.Count; i++)
            {
                Cell cell = table.Cell(i, 1);
                cell.Range.Text = headers[looper];
                if (looper == headers.Count - 1)
                {
                    AddHeaderFormatting(cell);
                }
                looper++;
            }
            return i;
        }
        private static int AddItemHeaders(ref Table table, int index)
        {
            List<string> headers = new List<string> { "Part Name", "Part No" };
            int looper = 0;
            int i = index;
            for (int i; i <= index + headers.Count; i++)
            {
                Cell cell = table.Cell(i, 1);
                cell.Range.Text = headers[looper];
                AddHeaderFormatting(cell);
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

        }

        #endregion
    }
}

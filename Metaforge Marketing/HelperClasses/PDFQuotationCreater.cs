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

                object saveOption = WdSaveOptions.wdDoNotSaveChanges;
                object originalFormat = WdOriginalFormat.wdOriginalDocumentFormat;
                object routeDocument = false;
                document.Close(ref saveOption, ref originalFormat, ref routeDocument);
                return path;
            }
            else
            {
                throw new Exception("Please Save the file");
            }
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
                Email email = new Email
                {
                    MailMessage = new System.Net.Mail.MailMessage(),
                };
                email.MailMessage.To.Add(recipient.Email);
                email.MailMessage.Attachments.Add(new System.Net.Mail.Attachment(path));
                email.MailMessage.Body = "";
                email.Send();
            } 
            catch(Exception ex)
            {
                
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

            QuotationTable = AddShortQuotationCostingTotals(ref QuotationTable, costings.ToList());

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
            if (table.Rows.Count != 6) { throw new Exception("Row count is not 6!"); }
            table.Cell(1, 1).Range.Text = "Item Name";
            table.Cell(2, 1).Range.Text = "Item Code";
            table.Cell(3, 1).Range.Text = "Total RM Cost";
            table.Cell(4, 1).Range.Text = "Total Conversion Cost";
            table.Cell(5, 1).Range.Text = "Add 15 % Profit";
            table.Cell(6, 1).Range.Text = "Total Cost Per Piece";
            
            // TODO: Figure out how to format the columns of the table,
            // and set the font to be bold and may be add a color as well
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
                table.Cell(1, i).Range.Text = costings[index].Item.ItemName;
                table.Cell(2, i).Range.Text = costings[index].Item.ItemCode;
                table.Cell(3, i).Range.Text = costings[index].RMCosting.CostPerPiece.ToString();
                table.Cell(4, i).Range.Text = costings[index].ConvCosting.TotalCostPerPiece.ToString();
                table.Cell(5, i).Range.Text = costings[index].AddProfit().ToString();
                table.Cell(6, i).Range.Text = costings[index].ComputeTotalCost().ToString();
            }
            return table;
        }

        #endregion Short Quotation Methods
    }
}

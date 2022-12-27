using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Metaforge_Marketing.Models;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;

namespace Metaforge_Marketing.HelperClasses.Quotation
{
    public class PDFQuotationCreator
    {
        // Summary:
        //      Defines the common methods such as creating an application, saving the document, or formatting a cell


        #region Fields
        protected static object oMissing = System.Reflection.Missing.Value;
        protected static object oEndOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */
        #endregion Fields

        #region Common Methods (Shared By short & long quotation)
        protected static Application CreateApplication()
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
        protected static Document AddDocument(ref Application app)
        {
            Document doc = app.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            return doc;
        }
        
        // Summary:
        //      Adds the header to the given document
        protected static void AddDocumentHeader(ref Document doc, string header)
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
        protected static string SaveQuotation(ref Document document)
        {
            //Save the document
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "PDF |*.pdf"
            };
            bool res = (bool)dialog.ShowDialog();
            try
            {
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
            finally
            {
                object saveOption = WdSaveOptions.wdDoNotSaveChanges;
                object originalFormat = WdOriginalFormat.wdOriginalDocumentFormat;
                object routeDocument = false;
                document.Close(ref saveOption, ref originalFormat, ref routeDocument);
                document.Application.Quit();
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

        // Summary:
        //      Formats the headers- Makes the font bold, and sets the background color as well
        //      Used to format the Item Code, Item Name, and Total Cost
        protected static void AddHeaderFormatting(ref Cell cell)
        {
            cell.Range.Font.Bold = 1; // Set font weight
            cell.Range.Font.Name = "verdana"; // Set font
            cell.Range.Font.Size = 8;         // Set font size  
            cell.Shading.BackgroundPatternColor = WdColor.wdColorGray25;
            cell.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter; //Center alignment for the Header cells
            cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter; //Center alignment for the Header cells
        }
        #endregion Common Methods
    }
}

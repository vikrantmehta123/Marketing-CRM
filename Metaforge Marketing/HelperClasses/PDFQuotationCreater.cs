
using System.Collections.Generic;
using Metaforge_Marketing.Models;
using Microsoft.Office.Interop.Word;

namespace Metaforge_Marketing.HelperClasses
{
    public class PDFQuotationCreater
    {
        private static Application CreateApplication()
        {
            Application app = new Application
            {
                Visible = true,
                ShowAnimation = false
            };
            return app;
        }
        
        private static void AddHeader(Document doc, string header)
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

        private static void AddQuotationTable(Document doc, IEnumerable<Costing> costings)
        {
            
        }
    }
}

using Metaforge_Marketing.Models;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Metaforge_Marketing.HelperClasses.Quotation
{
    public class BajajQuotationCreator :PDFQuotationCreator
    {
        private static void AddHeader(ref Document doc, Models.Quotation costing)
        {
            Paragraph header = doc.Paragraphs.Add();
            header.Range.Text = $"Item Name: {costing.Item.ItemName}\n" +
                                $"Item Code: {costing.Item.ItemCode}";
            header.Range.Font.Bold = 1;
        } 

        private static void AddItemSpecs(ref Document doc, Models.Quotation costing)
        {
            Paragraph ItemSpecs = doc.Paragraphs.Add();
            ItemSpecs.Range.Text = "A) Item Specs";
            ItemSpecs.Range.InsertParagraphAfter();
            
        }
    }
}

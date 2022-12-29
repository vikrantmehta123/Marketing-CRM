using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metaforge_Marketing.HelperClasses.Quotation
{
    public class QuotationSender
    {
        #region Fields
        private const string _file = "some path to the file";
        private IEnumerable<Costing> _preparedCostings;
        private IEnumerable<Item> _regrettedItems;
        private QuotationFormatEnum _quotationFormat;
        private IEnumerable<Buyer> _recipients;
        #endregion Methods

        #region Methods
        public static void SendQuotation(string path, IEnumerable<Item> regrettedItems, IEnumerable<Buyer> recipients, QuotationFormatEnum quotationFormat)
        {
            Email email = new Email();
            recipients.ToList().ForEach( buyer => email.MailMessage.To.Add(buyer.Email)); // Add the recipients to the list

            // TODO: Add the body to the email
            email.MailMessage.Body = "";

            if (!String.IsNullOrEmpty(path))
            {
                email.MailMessage.Attachments.Add(new System.Net.Mail.Attachment(path));
            }
            if(regrettedItems.Count() > 0)
            {
                regrettedItems.ToList().ForEach(item => email.MailMessage.Body += item.ItemName + ", ");
            }

            email.Send();
        }

        #endregion Methods

    }
}

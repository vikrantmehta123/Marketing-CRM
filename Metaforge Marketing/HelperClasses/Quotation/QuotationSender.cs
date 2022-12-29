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
        private const string PATH_TO_TERMS_AND_CONDITIONS = "SOME PATH";
        #endregion Methods

        #region Methods

        // Summary:
        //      Sends the Quotation to the Buyer
        // Parameters:
        //      List- Regretted Items list
        //      path- The path of the Quotation document
        public static void SendQuotation(string path, IEnumerable<Item> regrettedItems, IEnumerable<Buyer> recipients)
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
            email.MailMessage.Attachments.Add(new System.Net.Mail.Attachment(PATH_TO_TERMS_AND_CONDITIONS)); // Add the terms and conditions document
            email.Send();
        }

        #endregion Methods

    }
}

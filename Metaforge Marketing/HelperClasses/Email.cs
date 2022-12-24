using Metaforge_Marketing.Models;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace Metaforge_Marketing.HelperClasses
{
    public class Email
    {
        #region Fields
        private string Username = Environment.GetEnvironmentVariable("My_email");
        private string Password = Environment.GetEnvironmentVariable("My_email_password");
        #endregion Fields

        #region Properties
        public MailMessage MailMessage { get; set; }
        public string SelectedBlueprint { get; set; }
        public List<string> Blueprints { get; } = new List<string> { "Introductory", "Achievement" };
        #endregion Proeprties

        public Email()
        {
            MailMessage = new MailMessage();
            MailMessage.From = new MailAddress(Username);
        }


        /// <summary>
        /// Adds a list of buyers as mail recipients
        /// </summary>
        /// <param name="buyerList"></param>
        public void AddRecipients(IEnumerable<Buyer> buyerList)
        {
            foreach (Buyer buyer in buyerList)
            {
                if (String.IsNullOrEmpty(buyer.Email)) { continue; }
                else { MailMessage.To.Add(new MailAddress(buyer.Email)); }
            }
        }
        /// <summary>
        /// Logs into the account
        /// </summary>
        /// <returns>Email client</returns>
        private SmtpClient EstablishConnection()
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(Username, Password),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };
            return client;
        }

        /// <summary>
        /// Sends the mail
        /// </summary>
        public void Send()
        {
            SmtpClient client = EstablishConnection();
            try { client.Send(MailMessage); }
            finally { MailMessage.Dispose(); }
        }

        /// <summary>
        /// Reads the blueprint file
        /// </summary>
        /// <param name="blueprint"></param>
        /// <returns>Note of the file</returns>
        private string GetBlueprintText(string blueprint)
        {
            string PathToBlueprintFolder = @"D:\Metaforge\Metaforge\Mail Blueprints";
            string path = Path.Combine(PathToBlueprintFolder, blueprint + ".txt");
            string text = File.ReadAllText(path);
            return text;
        }

        /// <summary>
        /// Basic validation
        /// </summary>
        /// <returns></returns>
        public bool IsDataValid()
        {
            if (String.IsNullOrEmpty(MailMessage.Body)) { return false; }
            if (String.IsNullOrEmpty(MailMessage.Subject)) { return false; }
            return true;
        }
    }
}

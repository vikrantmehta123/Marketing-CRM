using Metaforge_Marketing.Models;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Windows;
using System.Linq;

namespace Metaforge_Marketing.HelperClasses
{
    public class Email : ModelsBase
    {
        #region Fields
        private readonly string Username = Environment.GetEnvironmentVariable("My_email");
        private readonly string Password = Environment.GetEnvironmentVariable("My_email_password");

        private string _selectedBlueprint;
        #endregion Fields

        #region Properties
        public MailMessage MailMessage { get; set; }
        public string SelectedBlueprint
        {
            get { return _selectedBlueprint; }
            set 
            { 
                _selectedBlueprint = value;
                if (_selectedBlueprint != null)
                {
                    MailMessage.Body = GetBlueprintText(_selectedBlueprint);
                    OnPropertyChanged(nameof(MailMessage.Body));
                }
            }
        }

        public List<string> Blueprints { get; } = new List<string> { "Introductory", "Achievement" };
        #endregion Proeprties

        public Email()
        {
            MailMessage = new MailMessage
            {
                From = new MailAddress(Username)
            };
        }

        #region Methods
        // Summary:
        //      Logs into account and returns the Email Client
        private SmtpClient EstablishConnection()
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Username, Password)
            };
            return client;
        }


        // Summary:
        //      Sends the mail
        public void Send()
        {
            var client = EstablishConnection();
            try { client.Send(MailMessage); }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally { MailMessage.Dispose(); }
        }

        // Summary:
        //      Reads the blueprint .txt file and returns the text in it
        private string GetBlueprintText(string blueprint)
        {
            string PathToBlueprintFolder = @"D:\Metaforge\Metaforge Marketing\Metaforge Marketing\MailBlueprints";
            string path = Path.Combine(PathToBlueprintFolder, blueprint + ".txt");
            string text = File.ReadAllText(path);
            return text;
        }


        // Summary: 
        //      Basic Validation
        public bool IsDataValid()
        {
            if (String.IsNullOrEmpty(MailMessage.Body)) { return false; }
            if (String.IsNullOrEmpty(MailMessage.Subject)) { return false; }
            return true;
        }


        public string GetQuotationMailText(List<Item> regrettedItems, List<Item> preparedItems)
        {
            string PathToBlueprintFolder = @"D:\Metaforge\Metaforge Marketing\Metaforge Marketing\MailBlueprints";
            string text;
            if (regrettedItems.Count == 0)
            {
                text = File.ReadAllText(Path.Combine(PathToBlueprintFolder, "QuoteAllMail.txt"));
            }
            else if(preparedItems.Count == 0)
            {
                text = File.ReadAllText(Path.Combine(PathToBlueprintFolder, "RegretAllMail.txt"));
            }
            else
            {
                string regret = WriteItems(regrettedItems);
                string prepared = WriteItems(preparedItems);

                string path = Path.Combine(PathToBlueprintFolder, "QuotationMail.txt");
                text = File.ReadAllText(path);
                text = text.Replace("{Regretted}", regret);
                text = text.Replace("{Prepared}", prepared);
            }
            return text;
        }

        private string WriteItems(List<Item> items)
        {
            string text = "";
            for (int i = 0; i < items.Count; i++)
            {
                if(i < items.Count - 1)
                {
                    text += items[i].ItemName + ", ";
                }
                else
                {
                    text += items[i].ItemName;
                }
            }
            return text;
        }
        #endregion Methods
    }
}

using Metaforge_Marketing.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Windows.Input;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.Models.Enums;
using System;
using System.Linq;
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.HelperClasses.Quotation;
using System.Windows;
using Metaforge_Marketing.HelperClasses.Pagination;

namespace Metaforge_Marketing.ViewModels.Send
{
    public class SendQuotationViewModel : ViewModelBase
    {
        public static IEnumerable<QuotationFormatEnum> QuotationFormats { get; } = Enum.GetValues(typeof(QuotationFormatEnum)).Cast<QuotationFormatEnum>();
        #region Fields
        private ICommand _sendQuoteCommand;
        private RFQ _selectedRFQ;
        private QuotationFormatEnum _selectedQuotationFormat;
        #endregion Fields

        #region Properties
        public QuotationFormatEnum SelectedQuotationFormat 
        { 
            get { return _selectedQuotationFormat; } 
            set { _selectedQuotationFormat = value; } 
        }
        public NormalPagination<RFQ> Pagination { get; set; }

        public RFQ SelectedRFQ
        {
            get { return _selectedRFQ;}
            set { _selectedRFQ = value;}
        }
        public ICommand SendQuoteCommand
        {
            get
            {
                if (_sendQuoteCommand== null)
                {
                    _sendQuoteCommand = new Command(p => SendQuotation());
                }
                return _sendQuoteCommand;
            }
        }

        #endregion Properties

        public SendQuotationViewModel()
        {
            Pagination = new NormalPagination<RFQ>(RFQsRepository.FetchQuotationReadyRFQs,RFQsRepository.CountQuotationReadyRFQs, Filter);
        }


        #region Methods
        // Summary:
        //      Filters the results on the screen based on Customer's Name and Project's Name
        private bool Filter(object o)
        {
            RFQ rfq = o as RFQ;
            if (Pagination != null)
            {
                if (string.IsNullOrEmpty(Pagination.PageSearchText)) { return true; }
                else
                {
                    return (rfq.ProjectName.Contains(Pagination.PageSearchText) || rfq.Customer.CustomerName.Contains(Pagination.PageSearchText));
                }
            }
            else { return true; }
        }


        // Summary:
        //      The function to execute when the Send Quotation Button is clicked
        //      Does all the hidden work- fetching RFQ Items, its costing,
        //      regretted items, creates a word document based on the format selected, and sends it to the buyer in question
        private void SendQuotation()
        {
            List<Item> RegrettedItems;
            List<Item> CostingPreparedItems;
            List<Quotation> PreparedCostings = new List<Quotation>();
            string path = "";
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
            {
                conn.Open();
                SelectedRFQ.Items = new List<Item>(ItemsRepository.FetchItems(conn, SelectedRFQ));
                RegrettedItems = (List<Item>)ItemsRepository.FetchItems(conn, SelectedRFQ, ItemStatusEnum.Regretted);
                CostingPreparedItems = (List<Item>)ItemsRepository.FetchItems(conn, SelectedRFQ, ItemStatusEnum.Customer_Costing_Prepared);
                foreach (Item item in CostingPreparedItems)
                {
                    PreparedCostings.Add(QuotationRepository.FetchLatestQuotation(conn, item));
                }
                conn.Close();
            }
            if (PreparedCostings.Count > 0)
            {
                if (SelectedQuotationFormat == QuotationFormatEnum.Short)
                {
                    try
                    {
                        path = ShortQuotationCreator.CreateQuotation(PreparedCostings);
                    }
                    catch(Exception ex) { MessageBox.Show(ex.Message); }
                    
                }
                else if (SelectedQuotationFormat == QuotationFormatEnum.Long)
                {
                    path = LongQuotationCreator.CreateQuotation(PreparedCostings);
                }
            }
            try
            {
                if (String.IsNullOrEmpty(path)) { throw new Exception("Quotation isn't saved"); }
                QuotationSender.SendQuotation(path, CostingPreparedItems, RegrettedItems, new List<Buyer> { SelectedRFQ.Buyer });
                RegrettedItems.ToList().ForEach(item => item.Status = ItemStatusEnum.RegretMailSent); // Mark the status of the the regretted items as regret mail sent
                CostingPreparedItems.ToList().ForEach(item => item.Status = ItemStatusEnum.QuotationSent); // Mark the status of the Quotation items as Quotation sent
                //UpdateDatabase(RegrettedItems);

                //TODO: Make sure that you include the Insert Item History Function too

                //UpdateDatabase(CostingPreparedItems);
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }

        private void UpdateDatabase(IEnumerable<Item> items)
        {
            using(SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
            {
                conn.Open();
                try
                {
                    foreach (var item in items)
                    {
                        //ItemsRepository.UpdateItemStatus(conn, item);
                    }
                }
                finally
                {
                    conn.Close();
                }
                
            }

        }
        #endregion Methods
    }
}

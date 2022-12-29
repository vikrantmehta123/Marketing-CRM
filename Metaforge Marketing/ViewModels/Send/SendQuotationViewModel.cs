using Metaforge_Marketing.HelperClasses.Commands;
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
        public PaginationCommands<RFQ> Pagination { get; set; }

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
            Pagination = new PaginationCommands<RFQ>(RFQsRepository.FetchQuotationReadyRFQs,RFQsRepository.CountQuotationReadyRFQs, filter);
        }


        #region Methods
        // Summary:
        //      Filters the results on the screen based on Customer's Name and Project's Name
        private bool filter(object o)
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
            IEnumerable<Item> RegrettedItems;
            IEnumerable<Costing> CostingPreparedItems;
            string path = "";
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
            {
                conn.Open();
                RegrettedItems = ItemsRepository.FetchItems(conn, SelectedRFQ, ItemStatusEnum.Regretted);
                CostingPreparedItems = CostingRepository.FetchCostings(conn, SelectedRFQ, CostingCategoryEnum.CustomerQuoted); //:=> A query to fetch all items with their costings, whose status = CustomerCostingPrepared
                conn.Close();
            }
            if (CostingPreparedItems.Count() > 0)
            {
                if (SelectedQuotationFormat == QuotationFormatEnum.Short)
                {
                    path = ShortQuotationCreator.CreateQuotation(CostingPreparedItems);
                }
                else if (SelectedQuotationFormat == QuotationFormatEnum.Long)
                {
                    path = LongQuotationCreator.CreateQuotation(CostingPreparedItems.ToList());
                }
            }

            QuotationSender.SendQuotation(path, RegrettedItems, new List<Buyer> { SelectedRFQ.Buyer });

            //TODO: Update the status of the Items as Mail sent
        }
        #endregion Methods
    }
}

using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.HelperClasses.Commands;
using Metaforge_Marketing.HelperClasses.Converters;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.ViewModels.Shared;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.RFQs
{
    internal class PendingRFQViewModel : SharedViewModelBase
    {
        #region Fields
        private ICommand _showDetailsCommand;
        private PaginationCommands<RFQ> _paginationCommands;
        #endregion Fields

        public PaginationCommands<RFQ> PaginationCommands
        {
            get { return _paginationCommands; }
            private set { _paginationCommands = value; }
        }

        public ICommand ShowDetailsCommand
        {
            get 
            { 
                if (_showDetailsCommand == null)
                {
                    _showDetailsCommand = new Command(p =>
                    {
                        new PopupWindowViewModel().Show(new DetailedRFQViewModel());
                    });
                }
                return _showDetailsCommand; 
            }
        }


        #region Constructors
        public PendingRFQViewModel()
        {
            // Populate the collection and instantiate the Pagination instance
            PaginationCommands= new PaginationCommands<RFQ>();
            int count;
            IEnumerable<RFQ> rfqs;
            int pendingItemStatus = ((int)ItemStatusEnum.Pending);
            int entriesPerPage = PaginationCommands.EntriesPerPage;
            int offsetIndex = 0;
            using(SqlConnection connection = new SqlConnection(Properties.Settings.Default.conn_string))
            {
                connection.Open();
                rfqs = RFQsRepository.FetchRFQs(connection, pendingItemStatus, offsetIndex, entriesPerPage);
                count = RFQsRepository.CountRFQs(connection, pendingItemStatus);
                connection.Close();
            }
            PaginationCommands = new PaginationCommands<RFQ>(rfqs, count, filter);
        }
        #endregion Constructors

        #region Methods
        private bool filter(object o)
        {
            RFQ rfq = o as RFQ;
            if (PaginationCommands != null)
            {
                if (string.IsNullOrEmpty(PaginationCommands.PageSearchText)) { return true; }
                else
                {
                    return (rfq.ProjectName.Contains(PaginationCommands.PageSearchText) || rfq.Customer.CustomerName.Contains(PaginationCommands.PageSearchText));
                }
            }
            else { return true; }
        }
        #endregion Methods
    }
}

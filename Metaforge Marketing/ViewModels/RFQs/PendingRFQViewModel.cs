using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.HelperClasses.Pagination;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.ViewModels.Shared;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.RFQs
{
    internal class PendingRFQViewModel : SharedViewModelBase
    {
        #region Fields
        private ICommand _showDetailsCommand;
        private NormalPagination<RFQ> _paginationCommands;
        #endregion Fields

        public NormalPagination<RFQ> PaginationCommands
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
            PaginationCommands= new NormalPagination<RFQ>(RFQsRepository.FetchPendingRFQs, RFQsRepository.CountPendingRFQs, filter);
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

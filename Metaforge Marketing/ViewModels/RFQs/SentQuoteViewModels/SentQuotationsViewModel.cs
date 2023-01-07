
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.HelperClasses.Pagination;
using Metaforge_Marketing.Models;
using System.Windows.Input;
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.ViewModels.Shared;

namespace Metaforge_Marketing.ViewModels.RFQs.SentQuoteViewModels
{
    public class SentQuotationsViewModel : SentQuotationsContainer
    {
        #region Fields
        private ICommand _showDetailsCommand;
        private NormalPagination<RFQ> _pagination;
        #endregion Fields

        public SentQuotationsViewModel()
        {
            _pagination = new NormalPagination<RFQ>(RFQsRepository.FetchQuotationSentRFQs, RFQsRepository.CountQuotationSentRFQs, filter);
        }


        #region Properties
        public NormalPagination<RFQ> Pagination { get { return _pagination; } }
        public ICommand ShowDetailsCommand
        {
            get
            {
                if (_showDetailsCommand == null)
                {
                    _showDetailsCommand = new Command(p => new PopupWindowViewModel().Show(new ShowDetailsViewModel()),p => SelectedRFQ != null);
                }
                return _showDetailsCommand;
            }
        }

        #endregion Properties


        #region Methods
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
        #endregion Methods
    }
}

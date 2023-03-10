
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.ViewModels.Add;
using Metaforge_Marketing.ViewModels.Reports;
using Metaforge_Marketing.ViewModels.RFQs;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string WindowTitle { get; } = "Home Page:";

        #region Fields
        private ICommand _addCustomerCommand, _addBuyerCommand, _addRFQCommand, _addRemarkCommand;
        private ICommand _prepareCostingCommand, _pendingRFQsCommand, _addPODetailsCommand, _sentRFQsCommand;
        private ICommand _customerHistoryCommand, _costingComparisonCommand, _performanceReportCommand, _quotationHistoryCommand;
        private ICommand _sendGeneralMailCommand, _sendQuotationMailCommand;
        private ICommand _updateRMMasterCommand, _adminMasterCommand, _operationsMasterCommand, _buyerMasterCommand, _customerMasterCommand, _itemMasterCommand;
        #endregion Fields

        public MainWindowViewModel()
        {
            CurrentPageViewModel = new PendingRFQViewModel();        }

        #region Commands

        #region Add Commands
        public ICommand AddCustomerCommand
        {
            get
            {
                if (_addCustomerCommand== null)
                {
                    _addCustomerCommand = new Command(p => ChangeViewModel(new ViewModels.Add.AddCustomerViewModel()));
                }
                return _addCustomerCommand;
            }
        }

        public ICommand AddBuyerCommand
        {
            get
            {
                if (_addBuyerCommand == null)
                {
                    _addBuyerCommand = new Command(p => ChangeViewModel(new ViewModels.Add.AddBuyerViewModel()));
                }
                return _addBuyerCommand;
            }
        }

        public ICommand AddRFQCommand
        {
            get
            {
                if (_addRFQCommand == null)
                {
                    _addRFQCommand = new Command(p => ChangeViewModel(new Add.AddRFQViewModel()));
                }
                return _addRFQCommand;
            }
        }
        public ICommand AddRemarkCommand
        {
            get
            {
                if (_addRemarkCommand == null)
                {
                    _addRemarkCommand = new Command(p => ChangeViewModel(new Add.AddRemarkViewModel()));
                }
                return _addRemarkCommand;
            }
        }
        #endregion

        #region RFQ Commands
        public ICommand AddPODetailsCommand
        {
            get
            {
                if (_addPODetailsCommand == null)
                {
                    _addPODetailsCommand = new Command(p => ChangeViewModel(new RFQs.POViewModels.AddPODetailsViewModel()));
                }
                return _addPODetailsCommand;
            }
        }
        public ICommand PrepareCostingCommand
        {
            get
            {
                if (_prepareCostingCommand == null)
                {
                    _prepareCostingCommand = new Command(p => ChangeViewModel(new CostingViewModel()));
                }
                return _prepareCostingCommand;
            }
        }
        public ICommand PendingRFQsCommand
        {
            get
            {
                if (_pendingRFQsCommand == null)
                {
                    _pendingRFQsCommand = new Command(p => ChangeViewModel(new PendingRFQViewModel()));
                }return _pendingRFQsCommand;
            }
        }
        public ICommand SentRFQsCommand
        {
            get
            {
                if (_sentRFQsCommand == null)
                {
                    _sentRFQsCommand = new Command(p => ChangeViewModel(new RFQs.SentQuoteViewModels.SentQuotationsViewModel()));
                }
                return _sentRFQsCommand;
            }
        }
        #endregion RFQ Commands

        #region Reports Commands
        public ICommand CostingComparisonCommand
        {
            get
            {
                if (_costingComparisonCommand== null)
                {
                    _costingComparisonCommand = new Command(p => ChangeViewModel(new Reports.CostingComparisonViewModel()));
                }
                return _costingComparisonCommand;
            }
        }

        public ICommand PerformanceReportCommand
        {
            get
            {
                if (_performanceReportCommand == null)
                {
                    _performanceReportCommand = new Command(p => ChangeViewModel(new Reports.PerformanceReportViewModel()));
                }
                return (_performanceReportCommand);
            }
        }
        public ICommand CustomerHistoryCommand
        {
            get
            {
                if (_customerHistoryCommand == null)
                {
                    _customerHistoryCommand = new Command(p => ChangeViewModel(new Reports.CustomerHistoryViewModel()));
                }
                return _customerHistoryCommand; 
            }
        }

        public ICommand QuotationHistoryCommand
        {
            get
            {
                if (_quotationHistoryCommand == null)
                {
                    _quotationHistoryCommand = new Command(p => ChangeViewModel(new QuotationHistoryViewModel()));
                }
                return _quotationHistoryCommand;
            }
        }
        #endregion Reports Commands

        #region Mailing Commands
        public ICommand SendGeneralMailCommand
        {
            get
            {
                if (_sendGeneralMailCommand == null)
                {
                    _sendGeneralMailCommand = new Command(p => ChangeViewModel(new Send.SendGeneralMailViewModel()));
                }
                return _sendGeneralMailCommand;
            }
        }

        public ICommand SendQuotationMailCommand
        {
            get
            {
                if (_sendQuotationMailCommand == null)
                {
                    _sendQuotationMailCommand = new Command(p => ChangeViewModel(new Send.SendQuotationViewModel()));
                }
                return _sendQuotationMailCommand;
            }
        }
        #endregion Mailing Commands

        #region Master Commands
        public ICommand UpdateRMMasterCommand
        {
            get
            {
                if (_updateRMMasterCommand == null)
                {
                    _updateRMMasterCommand = new Command(p => ChangeViewModel(new Masters.UpdateRMMasterViewModel()));
                }
                return _updateRMMasterCommand;
            }
        }

        public ICommand AdminMasterCommand
        {
            get
            {
                if(_adminMasterCommand == null)
                {
                    _adminMasterCommand = new Command(p => ChangeViewModel(new Masters.AdminMasterViewModel()));
                }
                return _adminMasterCommand;
            }
        }

        public ICommand OperationsMasterCommand
        {
            get
            {
                if(_operationsMasterCommand==null)
                {
                    _operationsMasterCommand = new Command(p => ChangeViewModel(new Masters.OperationsMasterViewModel()));
                }
                return _operationsMasterCommand;
            }
        }

        public ICommand BuyerMasterCommand
        {
            get
            {
                if(_buyerMasterCommand == null)
                {
                    _buyerMasterCommand = new Command(p => ChangeViewModel(new Masters.BuyerMasterViewModel()));
                }
                return _buyerMasterCommand;
            }
        }

        public ICommand CustomerMasterCommand
        {
            get
            {
                if (_customerMasterCommand == null)
                {
                    _customerMasterCommand = new Command(p => ChangeViewModel(new Masters.CustomerMasterViewModel()));
                }
                return _customerMasterCommand;
            }
        }

        public ICommand ItemMasterCommand
        {
            get
            {
                if(_itemMasterCommand == null)
                {
                    _itemMasterCommand = new Command(p => ChangeViewModel(new Masters.ItemMasterViewModel()));
                }
                return _itemMasterCommand;
            }
        }
        #endregion Master Commands

        #endregion Commands
    }
}

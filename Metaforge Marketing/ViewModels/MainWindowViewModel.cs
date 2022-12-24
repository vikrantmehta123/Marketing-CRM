
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.ViewModels.RFQs;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string WindowTitle { get; } = "Home Page:";

        #region Fields
        private ICommand _addCustomerCommand, _addBuyerCommand, _addRFQCommand, _addRemarkCommand;
        private ICommand _prepareCostingCommand, _pendingRFQsCommand;
        private ICommand _customerHistoryCommand, _costingComparisonCommand, _performanceReportCommand;
        private ICommand _sendGeneralMailCommand, _sendQuotationMailCommand;
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

        public ICommand PrepareCostingCommand
        {
            get
            {
                if (_prepareCostingCommand == null)
                {
                    _prepareCostingCommand = new Command(p => ChangeViewModel(new RFQs.CostingViewModel()));
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
                    _pendingRFQsCommand = new Command(p => ChangeViewModel(new ViewModels.RFQs.PendingRFQViewModel()));
                }return _pendingRFQsCommand;
            }
        }
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

        #endregion Commands
    }
}

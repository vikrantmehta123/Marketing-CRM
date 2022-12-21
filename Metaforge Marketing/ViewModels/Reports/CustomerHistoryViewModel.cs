

using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.ViewModels.Shared;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Reports
{
    public class CustomerHistoryViewModel : SharedViewModelBase
    {
        #region Fields
        private ObservableCollection<Remark> _remarks;
        private bool _isCustomerSelected;
        private ICommand _selectCustomerCommand;

        #endregion Fields


        #region Properties
        public ICommand SelectCustomerCommand
        {
            get
            {
                if (_selectCustomerCommand == null)
                {
                    _selectCustomerCommand = new Command(p => new PopupWindowViewModel().Show(new SelectCustomerViewModel()));
                }
                return _selectCustomerCommand;
            }
        }
        public bool IsCustomerSelected
        {
            get
            {
                _isCustomerSelected = (SelectedCustomer != null);
                return _isCustomerSelected;
            }
        }
        
        public ObservableCollection<Remark> Remarks
        {
            get 
            { 
                if (_remarks == null && SelectedCustomer != null)
                {
                    _remarks = new ObservableCollection<Remark>(SQLWrapper<Remark>.FetchWrapper<Customer>(Repository.RemarksRepository.FetchRemarks, SelectedCustomer));
                }
                return _remarks; 
            }
        }

        #endregion Properties

        public CustomerHistoryViewModel()
        {
            StaticPropertyChanged += CustomerSelected;
        }


        #region Methods
        /// <summary>
        /// Notifies the reqd properties when the customer is selected. 
        /// The UI then makes the dashboard visible, and the datagrid fetches the remarks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerSelected(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedCustomer))
            {
                OnPropertyChanged(nameof(IsCustomerSelected));
                OnPropertyChanged(nameof(Remarks));
            }
        }
        #endregion Methods
    }
}



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
        // Controls visibility of the dashboard
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
            SelectedCustomer = null; // Clear Selection on load
            StaticPropertyChanged += CustomerSelected;
        }


        #region Methods

        // When Customer is selected, notifies the Remarks property
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

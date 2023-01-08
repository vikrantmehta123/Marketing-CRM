using Metaforge_Marketing.Repository;
using Metaforge_Marketing.Models;
using System;
using System.Windows.Input;
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.HelperClasses.Pagination;

namespace Metaforge_Marketing.ViewModels.Shared
{
    public class SelectCustomerViewModel : PopupCloseMarker
    {
        #region Fields
        private ICommand _selectionDoneCommand;
        private NormalPagination<Customer> _pagination;
        #endregion Fields

        public SelectCustomerViewModel()
        {
            _pagination = new NormalPagination<Customer>(CustomersRepository.FetchCustomers, CustomersRepository.CountCustomers, CustomersRepository.FetchCustomers, Filter);
        }

        #region Properties
        public NormalPagination<Customer> PaginationCommands { get { return _pagination; } }
        public override ICommand SelectionDoneCommand
        {
            get
            {
                if (_selectionDoneCommand == null)
                {
                    _selectionDoneCommand = new Command(p => Close(p), p => IsSelectionDone());
                }
                return _selectionDoneCommand;
            }
        }
        #endregion Properties


        #region Methods
        private bool Filter(object o)
        {
            Customer cust = o as Customer;
            if (PaginationCommands != null)
            {
                if (string.IsNullOrEmpty(PaginationCommands.PageSearchText)) { return true; }
                else
                {
                    return (cust.CustomerName.Contains(PaginationCommands.PageSearchText));
                }
            }
            else { return true; }
        }
        public override void ClearSelection() { SelectedCustomer = null; }

        public override bool IsSelectionDone() { return SelectedCustomer != null; }
        #endregion Methods

    }
}

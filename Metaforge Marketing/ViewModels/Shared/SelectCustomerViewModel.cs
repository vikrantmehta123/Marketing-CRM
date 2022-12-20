using Metaforge_Marketing.Repository;
using Metaforge_Marketing.HelperClasses.Commands;
using Metaforge_Marketing.Models;
using System;
using System.Windows.Input;
using Metaforge_Marketing.HelperClasses;

namespace Metaforge_Marketing.ViewModels.Shared
{
    public class SelectCustomerViewModel : PopupCloseMarker
    {
        private ICommand _selectionDoneCommand;
        public PaginationCommands<Customer> PaginationCommands { get; private set; }

        public override ICommand SelectionDoneCommand
        {
            get
            {
                if (_selectionDoneCommand == null)
                {
                    _selectionDoneCommand = new Command(p => Close(p), p=> IsSelectionDone());
                }
                return _selectionDoneCommand;
            }
        }

        public override void ClearSelection() {  SelectedCustomer = null; }

        public override bool IsSelectionDone() { return SelectedCustomer != null; }

        private bool filter(object o)
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


        public SelectCustomerViewModel()
        {
            PaginationCommands = new PaginationCommands<Customer>(CustomersRepository.FetchCustomers, CustomersRepository.CountCustomers, CustomersRepository.FetchCustomers, filter);
        }

    }
}

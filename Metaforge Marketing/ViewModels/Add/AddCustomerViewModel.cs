

using Metaforge_Marketing.HelperClasses.Commands;
using Metaforge_Marketing.Models;

namespace Metaforge_Marketing.ViewModels.Add
{
    public class AddCustomerViewModel : ViewModelBase
    {
        private Customer _customerToAdd;
        private SaveCommand<Customer> _saveCommand;
        public Customer CustomerToAdd { get { return _customerToAdd; } set { _customerToAdd = value; } }
        public SaveCommand<Customer> SaveCommand { get { return _saveCommand; } }
        public AddCustomerViewModel()
        {
            _customerToAdd= new Customer();
            //_saveCommand = new SaveCommand<Customer>(Repository.CustomersRepository.InsertToDB, _customerToAdd);
        }
    }
}

using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.HelperClasses.Pagination;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;

namespace Metaforge_Marketing.ViewModels.Masters
{
    public class CustomerMasterViewModel : ViewModelBase
    {
        private MasterPagination<Customer> _pagination;

        public MasterPagination<Customer> Pagination
        {
            get { return _pagination; }
        }

        public CustomerMasterViewModel()
        {
            _pagination = new MasterPagination<Customer>(CustomersRepository.CountCustomers, CustomersRepository.UpdateDB, CustomersRepository.FetchCustomersIntoDataTable);
        }
    }
}

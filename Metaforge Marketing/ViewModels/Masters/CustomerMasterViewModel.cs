using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Repository;

namespace Metaforge_Marketing.ViewModels.Masters
{
    public class CustomerMasterViewModel : ViewModelBase
    {
        private MasterDataPagination _masterData;

        public MasterDataPagination MasterData
        {
            get { return _masterData; }
        }

        public CustomerMasterViewModel()
        {
            _masterData = new MasterDataPagination(CustomersRepository.UpdateDB, CustomersRepository.FetchCustomersIntoDataTable, CustomersRepository.CountCustomers);
        }
    }
}

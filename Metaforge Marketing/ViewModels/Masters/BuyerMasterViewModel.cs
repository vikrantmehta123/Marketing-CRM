using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Repository;

namespace Metaforge_Marketing.ViewModels.Masters
{
    public class BuyerMasterViewModel : ViewModelBase
    {
        private MasterDataPagination _masterData;

        public MasterDataPagination MasterData
        {
            get
            {
                return _masterData;
            }
        }

        public BuyerMasterViewModel()
        {
            _masterData = new MasterDataPagination(BuyersRepository.UpdateDB, BuyersRepository.FetchBuyersIntoDatatable, BuyersRepository.CountBuyers);
        }
    }
}

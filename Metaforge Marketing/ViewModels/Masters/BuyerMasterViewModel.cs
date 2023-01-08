using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.HelperClasses.Pagination;

namespace Metaforge_Marketing.ViewModels.Masters
{
    public class BuyerMasterViewModel : ViewModelBase
    {
        private MasterPagination<Buyer> _pagination;

        public MasterPagination<Buyer> Pagination { get { return _pagination; } }

        public BuyerMasterViewModel()
        {
            _pagination = new MasterPagination<Buyer>(BuyersRepository.CountBuyers, BuyersRepository.UpdateDB, BuyersRepository.FetchBuyersIntoDatatable);
        }
    }
}

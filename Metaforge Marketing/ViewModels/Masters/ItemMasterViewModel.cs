
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.HelperClasses;

namespace Metaforge_Marketing.ViewModels.Masters
{
    public class ItemMasterViewModel : ViewModelBase
    {
        private MasterDataPagination _masterData;

        public MasterDataPagination MasterData
        {
            get { return _masterData; }
        }

        public ItemMasterViewModel()
        {
            _masterData = new MasterDataPagination(ItemsRepository.UpdateDB, ItemsRepository.FetchItemsIntoDataTable, ItemsRepository.CountItems);
        }
    }
}

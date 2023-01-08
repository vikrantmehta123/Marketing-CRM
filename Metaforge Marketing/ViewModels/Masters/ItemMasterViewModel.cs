
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.HelperClasses.Pagination;
using Metaforge_Marketing.Models;

namespace Metaforge_Marketing.ViewModels.Masters
{
    public class ItemMasterViewModel : ViewModelBase
    {
        private MasterPagination<Item> _pagination;

        public MasterPagination<Item> Pagination
        {
            get { return _pagination; }
        }

        public ItemMasterViewModel()
        {
            _pagination = new MasterPagination<Item>(ItemsRepository.CountItems, ItemsRepository.UpdateDB, ItemsRepository.FetchItemsIntoDataTable);
        }
    }
}

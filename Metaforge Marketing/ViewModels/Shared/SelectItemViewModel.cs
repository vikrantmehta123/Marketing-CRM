using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using System.Windows.Input;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.HelperClasses.Pagination;

namespace Metaforge_Marketing.ViewModels.Shared
{
    public class SelectItemViewModel : PopupCloseMarker
    {
        #region Fields
        private ICommand _selectionDoneCommand;
        #endregion Fields

        public SelectItemViewModel()
        {
            PaginationCommands = new NormalPagination<Item>(ItemsRepository.FetchItems, ItemsRepository.CountItems, ItemsRepository.FetchItems, filter);
        }

        #region Properties
        public NormalPagination<Item> PaginationCommands { get; private set; }

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
        public override void ClearSelection() { SelectedItem = null; }
        public override bool IsSelectionDone() { return SelectedItem != null; }
        private bool filter(object o)
        {
            Item item = o as Item;
            if (PaginationCommands != null)
            {
                if (string.IsNullOrEmpty(PaginationCommands.PageSearchText)) { return true; }
                else
                {
                    return (item.ItemName.Contains(PaginationCommands.PageSearchText));
                }
            }
            else { return true; }
        }
        #endregion Methods
    }
}

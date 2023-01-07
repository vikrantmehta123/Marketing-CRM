using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.Models.Enums;
using System.Windows.Input;
using Metaforge_Marketing.HelperClasses;
using System.Windows.Controls;
using System.Windows;
using Metaforge_Marketing.HelperClasses.Pagination;

namespace Metaforge_Marketing.ViewModels.RFQs.POViewModels
{
    public class SelectPOItemViewModel : POContainer
    {
        #region Fields
        private NormalPagination<Item> _pagination;
        private ICommand _selectionDoneCommand;
        #endregion Fields

        #region Properties
        public NormalPagination<Item> Pagination
        {
            get { return _pagination; }
        }
        public ICommand SelectionDoneCommand
        {
            get
            {
                if (_selectionDoneCommand == null)
                {
                    _selectionDoneCommand = new Command(p => Window.GetWindow(((UserControl)p)).DialogResult = true, p => SelectedItem != null);
                }
                return _selectionDoneCommand;
            }
        }
        #endregion Properties

        public SelectPOItemViewModel()
        {
            _pagination = new NormalPagination<Item>(ItemsRepository.FetchItems, Filter, ItemsRepository.CountItems, ((int)ItemStatusEnum.QuotationSent));
        }

        #region Methods
        private bool Filter(object o)
        {
            Item item = o as Item;
            if (_pagination != null)
            {
                if (string.IsNullOrEmpty(_pagination.PageSearchText)) { return true; }
                else
                {
                    return (item.ItemName.Contains(_pagination.PageSearchText));
                }
            }
            else { return true; }
        }

        #endregion Methods
    }
}

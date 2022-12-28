using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.ViewModels.Add;
using Metaforge_Marketing.ViewModels.Shared;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Metaforge_Marketing.ViewModels.RFQs
{
    internal class DetailedRFQViewModel : PopupCloseMarker
    {
        #region Fields
        private ICommand _selectionDoneCommand, _addCostingCommand;
        private ObservableCollection<Item> _items;
        #endregion Fields

        #region Properties

        public ICommand AddCostingCommand
        {
            get
            {
                if (_addCostingCommand == null)
                {
                    _addCostingCommand = new Command(p =>
                    {
                        ChangeViewModel(new CostingViewModel());
                        Close(p);
                    });
                }
                return _addCostingCommand;
            }
        }
        public override ICommand SelectionDoneCommand
        {
            get
            {
                return _selectionDoneCommand;
            }
        }
        public ObservableCollection<Item> Items
        {
            get
            {
                if (SelectedRFQ != null)
                {
                    _items = new ObservableCollection<Item>(SQLWrapper<Item>.FetchWrapper(Repository.ItemsRepository.FetchItems, SelectedRFQ));
                }
                return _items;
            }
        }
        #endregion Properties

        public override void ClearSelection() { SelectedItem = null; }

        public override bool IsSelectionDone() { return SelectedItem != null; }

    }
}

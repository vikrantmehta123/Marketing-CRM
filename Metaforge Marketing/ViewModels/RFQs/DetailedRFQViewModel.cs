using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.ViewModels.Shared;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.RFQs
{
    internal class DetailedRFQViewModel : PopupCloseMarker
    {
        #region Fields
        private ICommand _selectionDoneCommand;
        private ObservableCollection<Item> _items;
        #endregion Fields

        #region Properties
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

using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.ViewModels.Shared;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.RFQs
{
    internal class DetailedRFQViewModel : PopupCloseMarker
    {
        #region Fields
        private ICommand _selectionDoneCommand, _addCostingCommand, _saveCommand;
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

        public ICommand SaveCommand
        {
            get
            {

                return _saveCommand;
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

        private void Save()
        {
            foreach (Item item in _items)
            {
                if (item.Status != Models.Enums.ItemStatusEnum.Regretted && item.IsRegretted)
                {
                    using(SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        // TestRepository.UpdateItemStatus(conn, item, ItemStatusEnum.Regretted);
                        // InsertItemHistory(conn, item, DateTime.Today.Date, "Item regretted");
                        conn.Close();
                    }
                }
            }
        }

    }
}

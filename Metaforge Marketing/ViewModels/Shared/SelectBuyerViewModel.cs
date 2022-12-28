

using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Shared
{
    public class SelectBuyerViewModel : PopupCloseMarker
    {
        #region Fields
        private string conn_string = Properties.Settings.Default.conn_string;
        private ICommand _selectionDoneCommand, _clearCommand;
        private Customer _selectedCustomer;
        private ObservableCollection<Buyer> _buyers;
        #endregion Fields

        #region Properties
        public ObservableCollection<Buyer> Buyers
        {
            get
            {
                if (_buyers == null)
                {
                    using(SqlConnection conn = new SqlConnection(conn_string))
                    {
                        conn.Open();
                        _buyers = new ObservableCollection<Buyer>(BuyersRepository.FetchBuyers(conn, _selectedCustomer));
                        conn.Close();
                    }
                }
                return _buyers;
            }
        }
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
        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                {
                    _clearCommand = new Command(p => ClearSelection());
                }
                return _clearCommand;
            }
        }
        #endregion Properties

        public SelectBuyerViewModel(Customer selectedCustomer)
        {
            _selectedCustomer= selectedCustomer;
        }


        public override void ClearSelection() { SelectedBuyer= null; }

        public override bool IsSelectionDone() { return SelectedBuyer != null; }
    }
}

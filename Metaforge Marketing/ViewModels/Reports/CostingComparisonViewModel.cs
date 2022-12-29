
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.ViewModels.Shared;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Reports
{
    public class CostingComparisonViewModel : SharedViewModelBase
    {
        #region Fields
        private DataTable _rmCostings, _metaforgeCC, _customerApprovedCC, _customerQuotedCC;
        private ICommand _selectItemCommand;
        #endregion Fields

        #region Properties
        public ICommand SelectItemCommand
        {
            get
            {
                if(_selectItemCommand == null)
                {
                    _selectItemCommand = new Command(p => new PopupWindowViewModel().Show(new SelectItemViewModel()));
                }
                return _selectItemCommand;
            }
        }
        public DataTable RMCostings
        {
            get
            {
                if (SelectedItem!= null)
                {
                    using(SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        _rmCostings = CostingRepository.FetchRMCostingsIntoDatatable(conn, SelectedItem);
                        conn.Close();
                    }
                }
                return _rmCostings;
            }
        }

        public DataTable MetaforgeCC
        {
            get
            {
                if (SelectedItem != null)
                {
                    using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        _metaforgeCC = CostingRepository.FetchCCIntoDatatable(conn, SelectedItem, Models.Enums.CostingCategoryEnum.Metaforge);
                        conn.Close();
                    }
                }
                return _metaforgeCC;
            }
        }

        public DataTable CustomerApprovedCC
        {
            get
            {
                if (SelectedItem != null)
                {
                    using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        _customerApprovedCC = CostingRepository.FetchCCIntoDatatable(conn, SelectedItem, Models.Enums.CostingCategoryEnum.CustomerApproved);
                        conn.Close();
                    }
                }
                return _customerApprovedCC;
            }
        }

        public DataTable CustomerQuotedCC
        {
            get
            {
                if (SelectedItem != null)
                {
                    using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        _customerQuotedCC = CostingRepository.FetchCCIntoDatatable(conn, SelectedItem, Models.Enums.CostingCategoryEnum.CustomerQuoted);
                        conn.Close();
                    }
                }
                return _customerQuotedCC;
            }
        }

        #endregion Properties
        public CostingComparisonViewModel()
        {
            SelectedItem = null; // Clear selections on load
            StaticPropertyChanged += ItemSelectionHandler;
        }


        #region Methods
        private void ItemSelectionHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedItem))
            {
                OnPropertyChanged(nameof(RMCostings));
                OnPropertyChanged(nameof(MetaforgeCC));
                OnPropertyChanged(nameof(CustomerApprovedCC));
                OnPropertyChanged(nameof(CustomerQuotedCC));
            }
        }

        #endregion Methods
    }
}

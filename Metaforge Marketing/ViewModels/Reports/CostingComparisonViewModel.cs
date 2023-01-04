
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
        
        // Summary: 
        //      Fetch Raw Material Costings
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
                    OnPropertyChanged(nameof(ShowRMCostings));
                }
                return _rmCostings;
            }
        }

        // Summary:
        //      Fetch Metaforge's Conversion Quotation
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
                    OnPropertyChanged(nameof(ShowMetaforgeCC));
                }
                return _metaforgeCC;
            }
        }

        // Summary:
        //      Fetch the Customer Quoted Conversion Quotation
        public DataTable CustomerQuotedCC
        {
            get
            {
                if (SelectedItem != null)
                {
                    using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        _customerQuotedCC = CostingRepository.FetchCCIntoDatatable(conn, SelectedItem, Models.Enums.CostingCategoryEnum.Customer);
                        conn.Close();
                    }
                    OnPropertyChanged(nameof(ShowCustomerQuotedCC));
                }
                return _customerQuotedCC;
            }
        }

        #endregion Properties

        #region Visibility Controllers
        public bool ShowRMCostings
        {
            get
            {
                if (_rmCostings == null || _rmCostings.Rows.Count == 0) { return false; }
                return true;
            }
        }

        public bool ShowMetaforgeCC
        {
            get
            {
                if(_metaforgeCC == null || _metaforgeCC.Rows.Count == 0) { return false; }
                return true;
            }
        }
        public bool ShowCustomerQuotedCC
        {
            get
            {
                if (_customerQuotedCC == null || _customerQuotedCC.Rows.Count == 0) { return false; }
                return true;
            }
        }

        public bool ShowCustomerApprovedCC
        {
            get
            {
                if (_customerApprovedCC == null || _customerApprovedCC.Rows.Count == 0) { return false; }
                return true;
            }
        }
        #endregion Visibility Controllers

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
                OnPropertyChanged(nameof(CustomerQuotedCC));
            }
        }

        #endregion Methods
    }
}

using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.ViewModels.Shared;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Windows.Controls;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.RFQs
{
    public class CostingViewModel : SharedViewModelBase
    {
        #region Fields
        private RMCosting _rmCosting = new RMCosting() { RMConsidered = new RM() }, _mfRMCosting;
        private DataTable _convCosting = new DataTable(), _mfConvCosting;
        private ICommand _updateCommand, _selectItemCommand, _showMFCostingCommand;
        private CostingCategoryEnum _costingCategory;
        private string conn_string = Properties.Settings.Default.conn_string;
        private bool _canShowMFCosting;
        #endregion Fields

        #region Properties
        public bool CanShowMFCosting
        {
            get 
            {
                _canShowMFCosting = true;
                return _canShowMFCosting;
            }
        }
        public CostingCategoryEnum CostingCategory
        {
            get { return _costingCategory; }
            set
            {
                if (_costingCategory != value)
                {
                    _costingCategory = value;
                    if(SelectedItem != null)
                    {
                        OnPropertyChanged(nameof(RMCosting));
                        OnPropertyChanged(nameof(ConvCosting));
                        OnPropertyChanged(nameof(CanShowMFCosting));
                    }
                }
            }
        }

        public RMCosting RMCosting
        {
            get
            {
                if (SelectedItem != null && CostingCategory != CostingCategoryEnum.None)
                {
                    using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        _rmCosting = CostingRepository.FetchRMCosting(conn, SelectedItem, CostingCategory, new RMCosting());
                        conn.Close();

                    }
                }
                return _rmCosting;
            }
            set
            {
                if (_rmCosting != value)
                {
                    _rmCosting = value;
                }
            }
        }

        public DataView ConvCosting
        {
            get
            {
                if (SelectedItem != null && CostingCategory != CostingCategoryEnum.None)
                {
                    using (SqlConnection conn = new SqlConnection(conn_string))
                    {
                        conn.Open();
                        _convCosting = TestRepository.FetchConvCosting(conn, SelectedItem, CostingCategory);
                        conn.Close();
                    }
                }
                return _convCosting.DefaultView;
            }
        }
        public DataTable MFConvCosting
        {
            get 
            {
                if (SelectedItem != null && CostingCategory != CostingCategoryEnum.None)
                {
                    using (SqlConnection conn = new SqlConnection(conn_string))
                    {
                        conn.Open();
                        _mfConvCosting = TestRepository.FetchConvCosting(conn, SelectedItem, CostingCategoryEnum.Metaforge);
                        conn.Close();
                    }
                }
                return _mfConvCosting; 
            }
        }
        public RMCosting MFRMCosting
        {
            get 
            {
                if (SelectedItem != null && CostingCategory != CostingCategoryEnum.None)
                {
                    using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        _mfRMCosting = CostingRepository.FetchRMCosting(conn, SelectedItem, CostingCategoryEnum.Metaforge, new RMCosting());
                        conn.Close();

                    }
                }
                return _mfRMCosting; 
            }
        }
        #endregion Properties

        #region Commands
        public ICommand UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                {
                    _updateCommand = new Command(p =>
                    {
                        Costing costing = new Costing();
                        costing.RMCosting = _rmCosting;
                        using (SqlConnection conn = new SqlConnection(conn_string))
                        {
                            conn.Open();
                            try
                            {
                                TestRepository.InsertCosting(conn, _convCosting, SelectedItem, _rmCosting, CostingCategory);
                            }
                            finally
                            {
                                conn.Close();
                            }
                        }

                    });
                }
                return _updateCommand;
            }
        }

        public ICommand SelectItemCommand
        {
            get
            {
                if (_selectItemCommand == null)
                {
                    _selectItemCommand = new Command(p =>
                    {
                        new PopupWindowViewModel().Show(new SelectItemViewModel());
                    });
                }
                return _selectItemCommand;
            }
        }
        public ICommand ShowMFCostingCommand
        {
            get
            {
                if (_showMFCostingCommand == null)
                {
                }
                return _showMFCostingCommand;
            }
        }

        #endregion Commands
        #region Constructor
        public CostingViewModel()
        {
            StaticPropertyChanged += ItemSelectionHandler;
            SelectedItem = null;
        }
        #endregion Constructor

        #region Methods
        private void ItemSelectionHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName== nameof(SelectedItem))
            {
                if (CostingCategory !=  CostingCategoryEnum.None)
                {
                    OnPropertyChanged(nameof(ConvCosting));
                    OnPropertyChanged(nameof(RMCosting));
                    OnPropertyChanged(nameof(CanShowMFCosting));
                }
            }
        }

        #endregion Methods
    }
}

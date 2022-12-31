using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.ViewModels.Shared;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.RFQs
{
    public class CostingViewModel : SharedViewModelBase
    {
        #region Fields
        private Item _item = new Item() { Id = 3, Status = ItemStatusEnum.Pending };
        private RMCosting _rmCosting = new RMCosting() { RMConsidered = new RM() };
        private DataTable _convCosting;
        private ICommand _updateCommand;
        private CostingCategoryEnum _costingCategory;
        private string conn_string = Properties.Settings.Default.conn_string;
        #endregion Fields

        public CostingCategoryEnum CostingCategory
        {
            get { return _costingCategory; }
            set
            {
                if (_costingCategory != value)
                {
                    _costingCategory = value;
                    OnPropertyChanged(nameof(RMCosting));
                    OnPropertyChanged(nameof(ConvCosting));
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
                        _rmCosting = CostingRepository.FetchRMCosting(conn, _item, CostingCategory, _rmCosting);
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
                if(SelectedItem != null && CostingCategory != CostingCategoryEnum.None)
                {
                    using (SqlConnection conn = new SqlConnection(conn_string))
                    {
                        conn.Open();
                        _convCosting = TestRepository.FetchConvCosting(conn, _item, CostingCategory);
                        conn.Close();
                    }
                }
                return _convCosting.DefaultView;
            }
        }

        public ICommand UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                {
                    _updateCommand = new Command(p =>
                    {
                        Costing costing = new Costing
                        {
                            RMCosting = _rmCosting
                        };
                        using (SqlConnection conn = new SqlConnection(conn_string))
                        {
                            conn.Open();
                            try
                            {
                                TestRepository.InsertCosting(conn, _convCosting, _item, _rmCosting, CostingCategory);
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
    }
}

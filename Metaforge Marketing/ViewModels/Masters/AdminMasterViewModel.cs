using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Masters
{
    public class AdminMasterViewModel : ViewModelBase
    {
        private DataTable _adminMasterTable;
        private ICommand _updateCommand;

        public ICommand UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                {
                    _updateCommand = new Command(p =>
                    {
                        using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                        {
                            conn.Open();
                            AdminsRepository.UpdateAdminsMaster(conn, _adminMasterTable);
                            conn.Close();
                        }
                        MessageBox.Show("Successfully updated");
                    });
                }
                return _updateCommand;
            }
        }
        public DataTable AdminMasterTable
        {
            get
            {
                if (_adminMasterTable == null)
                {
                    using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        _adminMasterTable = AdminsRepository.FetchAdminsIntoDatatable(conn);
                        conn.Close();
                    }
                }
                return _adminMasterTable;
            }
        }
    }
}

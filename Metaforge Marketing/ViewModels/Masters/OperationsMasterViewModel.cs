using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System.Windows;
using System.Data;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Masters
{
    internal class OperationsMasterViewModel :ViewModelBase
    {
        private DataTable _operationMasterTable;
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
                            OperationsRepository.UpdateMaster(conn, _operationMasterTable);
                            conn.Close();
                        }
                        MessageBox.Show("Successfully updated");
                    });
                }
                return _updateCommand;
            }
        }
        public DataTable OperationMasterTable
        {
            get
            {
                if (_operationMasterTable == null)
                {
                    using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        _operationMasterTable = OperationsRepository.FetchOperationsIntoDatatable(conn);
                        conn.Close();
                    }
                }
                return _operationMasterTable;
            }
        }
    }
}

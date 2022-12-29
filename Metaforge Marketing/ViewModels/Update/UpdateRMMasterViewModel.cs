using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models.Enums;
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Update
{
    public class UpdateRMMasterViewModel : ViewModelBase
    {
        private DataTable _rmMasterData;
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
                            RMRepository.UpdateRMMaster(conn, RMMasterData);
                            conn.Close();
                        }
                        MessageBox.Show("Successfully updated");
                    });
                }
                return _updateCommand;
            }
        }
        public DataTable RMMasterData
        {
            get
            {
                if(_rmMasterData == null)
                {
                    using(SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        _rmMasterData = RMRepository.FetchRMsTable(conn);
                        conn.Close();
                    }
                }
                return _rmMasterData;
            }
        }
    }
}

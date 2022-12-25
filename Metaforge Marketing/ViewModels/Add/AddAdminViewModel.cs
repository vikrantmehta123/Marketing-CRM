using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Add
{
    public class AddAdminViewModel : ViewModelBase
    {
        private ICommand _saveCommand;
        private Admin _adminToAdd;

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new Command(p => Save());
                }
                return _saveCommand;
            }
        }

        public Admin AdminToAdd { get { return _adminToAdd; } set { _adminToAdd = value; } }

        private void Save()
        {
            using(SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
            {
                conn.Open();
                AdminsRepository.InsertToDB(conn, AdminToAdd);
                conn.Close();
            }
        }
    }
}

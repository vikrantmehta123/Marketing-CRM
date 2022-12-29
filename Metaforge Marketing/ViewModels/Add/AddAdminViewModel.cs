using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Add
{
    public class AddAdminViewModel : ViewModelBase
    {
        private ICommand _saveCommand;
        private Admin _adminToAdd = new Admin();

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new Command(p => Save(), p => CanSave());
                }
                return _saveCommand;
            }
        }

        public Admin AdminToAdd 
        { 
            get { return _adminToAdd; } 
            set { 
                _adminToAdd = value; 
                OnPropertyChanged(nameof(AdminToAdd));
            } 
        }

        private void Save()
        {
            using(SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
            {
                conn.Open();
                try
                {
                    AdminsRepository.InsertToDB(conn, AdminToAdd);
                    MessageBox.Show("Successfully inserted");
                    AdminToAdd = new Admin();
                }
                finally { conn.Close(); }
            }
        }
        private bool CanSave()
        {
            if (String.IsNullOrEmpty(AdminToAdd.Name)) { return false; }
            return true;
        }
    }
}

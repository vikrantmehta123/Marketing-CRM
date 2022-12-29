using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Add
{
    public class AddRMMasterDataViewModel : ViewModelBase
    {
        public static IEnumerable<RMCategoryEnum> RMCategories { get; } = Enum.GetValues(typeof(RMCategoryEnum)).Cast<RMCategoryEnum>();

        #region Fields
        private RM _rmToInsert = new RM();
        private ICommand _clearCommand, _saveCommand;
        #endregion Fields

        #region Properties
        public RM RMToInsert
        {
            get { return _rmToInsert; }
            set { 
                _rmToInsert = value;
                OnPropertyChanged(nameof(RMToInsert));
            }
        }

        public ICommand ClearCommand
        {
            get 
            { 
                if (_clearCommand == null)
                {
                    _clearCommand = new Command(p =>
                    {
                        RMToInsert = new RM();
                    });
                }
                return _clearCommand;
            }
        }
        public ICommand SaveCommand
        {
            get 
            { 
                if(_saveCommand == null)
                {
                    _saveCommand = new Command(p => Save(), p => CanSave());
                }
                return _saveCommand; 
            }
        }
        #endregion Properties


        #region Methods
        private void Save()
        {
            using(SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
            {
                conn.Open();
                try
                {
                    RMRepository.InsertToDB(conn, RMToInsert);
                    MessageBox.Show("Successfully inserted");
                }
                finally { conn.Close(); }
            }
        }
        private bool CanSave()
        {
            return RMToInsert.IsFormDataValid();
        }
        #endregion Methods
    }
}

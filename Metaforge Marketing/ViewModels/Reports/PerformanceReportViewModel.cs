
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Reports
{
    public class PerformanceReportViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<Admin> _admins;
        #endregion Fields

        #region Properties
        public ObservableCollection<Admin> Admins
        {
            get 
            { 
                if (_admins== null)
                {
                    _admins = new ObservableCollection<Admin>(SQLWrapper<Admin>.FetchWrapper(AdminsRepository.FetchAdmins));
                }
                return _admins; 
            }
        }

        #endregion Properties

        public PerformanceReportViewModel()
        {
            DateTime start = new DateTime(2022, 11, 1);
            DateTime end = DateTime.Today;
            using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
            {
                conn.Open();
                _admins = new ObservableCollection<Admin>(AdminsRepository.FetchPerformanceReview(conn, start, end));
                conn.Close();
            }
        }
    }
}

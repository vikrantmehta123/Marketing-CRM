
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Reports
{
    public class PerformanceReportViewModel : ViewModelBase
    {
        #region Fields
        private string conn_string = Metaforge_Marketing.Properties.Settings.Default.conn_string;
        private int _rfqCount = -1, _preparedQuotationsCount = -1, _convertedQuotationsCount = -1;
        private float _conversionRate, _avgResponseTime;
        private DateTime _startDate = DateTime.Today, _endDate = DateTime.Today;
        private ObservableCollection<Admin> _admins;
        private Admin _selectedAdmin;
        #endregion Fields

        #region Properties
        public DateTime StartDate
        {
            get { return _startDate; }
            set { 
                _startDate = value; 
                OnPropertyChanged(nameof(RFQCount));
                OnPropertyChanged(nameof(ConvertedQuotationsCount));
                OnPropertyChanged(nameof(PreparedQuotationsCount));
            }
        }
        public DateTime EndDate
        {
            get { return _endDate; }
            set { 
                _endDate = value;
                OnPropertyChanged(nameof(RFQCount));
                OnPropertyChanged(nameof(ConvertedQuotationsCount));
                OnPropertyChanged(nameof(PreparedQuotationsCount));
            }
        }
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

        public bool IsAdminSelected
        {
            get
            {
                if (_selectedAdmin != null) { return true; }
                return false;
            }
        }

        public Admin SelectedAdmin
        {
            get { return _selectedAdmin; }
            set {
                _selectedAdmin = value;
                OnPropertyChanged(nameof(SelectedAdmin));
                OnPropertyChanged(nameof(IsAdminSelected));
                OnPropertyChanged(nameof(RFQCount));
                OnPropertyChanged(nameof(ConvertedQuotationsCount));
                OnPropertyChanged(nameof(PreparedQuotationsCount));
            }
        }

        public int RFQCount
        {
            get
            {
                if (SelectedAdmin != null)
                {
                    using(SqlConnection conn = new SqlConnection(conn_string))
                    {
                        conn.Open();
                        _rfqCount = AdminsRepository.CountRFQs(conn, SelectedAdmin, StartDate, EndDate);
                        conn.Close();
                    }
                }
                return _rfqCount;
            }
        }

        public int PreparedQuotationsCount
        {
            get
            {
                if ( SelectedAdmin != null)
                {
                    using(SqlConnection conn = new SqlConnection(conn_string))
                    {
                        conn.Open();
                        _preparedQuotationsCount = AdminsRepository.CountPreparedQuotations(conn, SelectedAdmin, StartDate, EndDate);
                        conn.Close();
                    }
                }return _preparedQuotationsCount;
            }
        }
        public int ConvertedQuotationsCount
        {
            get
            {
                if (SelectedAdmin != null)
                {
                    using (SqlConnection conn = new SqlConnection(conn_string))
                    {
                        conn.Open();
                        _convertedQuotationsCount = AdminsRepository.CountConvertedQuotations(conn, SelectedAdmin, StartDate, EndDate);
                        conn.Close();
                    }
                }
                return _convertedQuotationsCount;
            }
        }
        #endregion Properties
    }
}

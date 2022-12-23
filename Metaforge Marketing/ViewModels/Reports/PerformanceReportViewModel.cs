
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using Microsoft.Data.SqlClient;
using System;
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
            Admin.StaticPropertyChanged += DateChangedHandler;
        }

        #region Methods
        private void DateChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Admin.StartDate) || e.PropertyName == nameof(Admin.EndDate))
            {
                OnPropertyChanged(nameof(Admin.RFQCount));
                OnPropertyChanged(nameof(Admin.ConvertedQuotationsCount));
                OnPropertyChanged(nameof(Admin.ConversionRate));
                OnPropertyChanged(nameof(Admin.AvgResponseTime));
                OnPropertyChanged(nameof(Admin.PreparedCostingsCount));
            }
        }
        #endregion Methods
    }
}

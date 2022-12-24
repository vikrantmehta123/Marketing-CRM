using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.ViewModels.Shared;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.RFQs
{
    public class CostingViewModel : SharedViewModelBase
    {
        #region Fields
        private Models.Costing _costing;
        private ICommand _saveCommand, _clearCommand, _selectItemCommand;
        #endregion Fields


        #region Properties
        public RM RMMaster { get; set; } = new RM();


        public bool ShowDetailedCostingForm
        {
            get
            {
                if (((int)Costing.Format) == 2) { return true; }
                return false;
            }
        }

        public bool ShowShortFormatForm
        {
            get
            {
                if (((int)Costing.Format) == 1) { return true; }
                return false;
            }
        }

        public Models.Costing Costing
        {
            get
            {
                return _costing;
            }
            set
            {
                if (_costing != value)
                {
                    _costing = value;
                }
            }
        }

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

        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                {
                    _clearCommand = new Command(p => Clear());
                }
                return _clearCommand;
            }
        }

        public ICommand SelectItemCommand
        {
            get
            {
                if (_selectItemCommand == null)
                {
                    _selectItemCommand = new Command(p => new PopupWindowViewModel().Show(new SelectItemViewModel()));
                }
                return _selectItemCommand;
            }
        }
        #endregion  Properties


        public CostingViewModel()
        {
            ClearAllSelections();
            _costing = new Models.Costing();
            _costing.PropertyChanged += FormatChangedHandler;
            StaticPropertyChanged += FormatChangedHandler;
        }

        #region Methods
        private void Save()
        {
            using(SqlConnection connection = new SqlConnection(Properties.Settings.Default.conn_string))
            {
                connection.Open();
                Repository.CostingRepository.InsertToDB(connection, Costing);
                connection.Close();
            }
        }

        private bool CanSave()
        {
            return Costing.IsDataValid();
        }

        private void Clear()
        {
            MessageBox.Show(Costing.RMCosting.RMConsidered.Id + "");
            Costing = new Models.Costing();
            OnPropertyChanged(nameof(Costing));
        }

        private void FormatChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Costing.Format))
            {
                OnPropertyChanged(nameof(ShowDetailedCostingForm));
                OnPropertyChanged(nameof(ShowShortFormatForm));
                OnPropertyChanged(nameof(Costing.RMCosting));
            }
            if (e.PropertyName == nameof(SelectedItem))
            {
                Costing.Item = SelectedItem;
                OnPropertyChanged(nameof(Costing.Item));
                OnPropertyChanged(nameof(Costing.RMCosting.RMRate));
            }
        }

        #endregion Methods
    }
}

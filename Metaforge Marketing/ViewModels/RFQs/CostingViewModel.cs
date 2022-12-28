using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.ViewModels.Shared;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.RFQs
{
    public class CostingViewModel : SharedViewModelBase
    {
        #region Fields
        private bool _showDetailedCostingForm, _showShortFormatForm;
        private Costing _costing;
        private RMCosting _rmCosting;
        private ConversionCosting _convCosting = new ConversionCosting();
        private ICommand _saveCommand, _clearCommand, _selectItemCommand;
        #endregion Fields


        #region Properties
        public RM RMMaster { get; set; } = new RM();

        // TODO: Move the below property to the Costing class, and change bindings
        public RMCosting RMCosting
        {
            get
            {
                if (SelectedItem != null && Costing.Category != Models.Enums.CostingCategoryEnum.None)
                {
                    using(SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        _rmCosting =  CostingRepository.FetchRMCosting(conn, SelectedItem, Costing.Category);
                        conn.Close();
                    }
                }
                return _rmCosting;
            }
            private set
            {
                if (_rmCosting != value)
                {
                    _rmCosting = value;
                    OnPropertyChanged(nameof(RMCosting));
                }
            }
        }

        public ConversionCosting ConvCosting
        {
            get
            {
                if (SelectedItem != null && Costing.Category != CostingCategoryEnum.None)
                {
                    using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        _convCosting = CostingRepository.FetchConversionCosting(conn, SelectedItem, Costing.Category);
                        conn.Close();
                    }
                }
                return _convCosting;
            }
            private set { 
                if(_convCosting != value)
                {
                    _convCosting = value;
                    OnPropertyChanged(nameof(ConvCosting));
                }
            }
        }

        public bool ShowDetailedCostingForm
        {
            get
            {
                if (((int)Costing.Format) == 2) { _showDetailedCostingForm = true; }
                else { _showDetailedCostingForm = false; }
                return _showDetailedCostingForm;
            }
            private set { _showDetailedCostingForm = value; }
        }

        public bool ShowShortFormatForm
        {
            get
            {
                if (((int)Costing.Format) == 1) { _showShortFormatForm = true; }
                else { _showShortFormatForm = false; }
                return _showShortFormatForm;
            }
            private set { _showShortFormatForm = value; }
        }

        public Costing Costing
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
            _costing = new Costing();
            _costing.PropertyChanged += FormatChangedHandler;
            
            StaticPropertyChanged += FormatChangedHandler;
        }

        #region Methods
        private void Save()
        {
            using(SqlConnection connection = new SqlConnection(Properties.Settings.Default.conn_string))
            {
                connection.Open();
                CostingRepository.InsertToDB(connection, Costing);
                connection.Close();
            }
        }

        private bool CanSave()
        {
            return true;
            //return Costing.IsDataValid();
        }

        private void Clear()
        {
            Costing = new Costing();
            SelectedItem = null;
            Costing.Format = CostingFormatEnum.None;
            ConvCosting = null; RMCosting = null;
            OnPropertyChanged(nameof(Costing));
        }

        private void FormatChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Costing.Format))
            {
                OnPropertyChanged(nameof(ShowDetailedCostingForm));
                OnPropertyChanged(nameof(ShowShortFormatForm));
            }
            if (e.PropertyName == nameof(SelectedItem))
            {
                Costing.Item = SelectedItem;
                OnPropertyChanged(nameof(Costing.Item));
                OnPropertyChanged(nameof(RMCosting));
                OnPropertyChanged(nameof(ConvCosting));
            }
            if (e.PropertyName == nameof(Costing.Category))
            {
                OnPropertyChanged(nameof(RMCosting));
                OnPropertyChanged(nameof(Costing.ConvCosting));
            }
            if(e.PropertyName == nameof(Machine.MCHr) || e.PropertyName == nameof(Machine.CycleTime) || e.PropertyName == nameof(Machine.Efficiency))
            {
                OnPropertyChanged(nameof(Machine.CostPerPiece));
                OnPropertyChanged(nameof(Operation.CostPerPiece));
            }
        }

        #endregion Methods
    }
}

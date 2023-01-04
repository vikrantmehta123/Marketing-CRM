using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.ViewModels.Shared;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.RFQs
{
    public class CostingViewModel : SharedViewModelBase
    {
        #region Fields
        private readonly string conn_string = Properties.Settings.Default.conn_string;
        private int _versionNumber = -2;
        private ObservableCollection<Operation> _operations;
        private Quotation _quotation = new Quotation();
        private RMCosting _rmCosting, _mfRMCosting;
        private DataTable _convCosting = new DataTable(), _mfConvCosting;
        private ICommand _selectItemCommand, _showMFCostingCommand,  _saveDraftCommand, _saveVersionCommand;
        private CostingCategoryEnum _costingCategory;
        private bool _canShowMFCosting, _showMFCosting;
        #endregion Fields

        #region Properties
        public int VersionNumber
        {
            get
            {
                return _versionNumber;
            }
        }

        public Quotation Quotation
        {
            get 
            { 
                return _quotation;
            }
            set { _quotation = value; }
        }
        public CostingCategoryEnum CostingCategory
        {
            get { return _costingCategory; }
            set
            {
                if (_costingCategory != value)
                {
                    _costingCategory = value;
                    if(SelectedItem != null)
                    {
                        _versionNumber = GetVersionNumber();
                        _quotation = GetQuotation();
                        RMCosting = GetRMCosting(_rmCosting);
                        OnPropertyChanged(nameof(ConvCosting));
                        OnPropertyChanged(nameof(CanShowMFCosting));
                    }
                }
            }
        }

        public RMCosting RMCosting
        {
            get { return _rmCosting; }
            set
            {
                if (_rmCosting != value)
                {
                    _rmCosting = value;
                    OnPropertyChanged(nameof(RMCosting));
                }
            }
        }

        public DataView ConvCosting
        {
            get
            {
                if (SelectedItem != null && CostingCategory != CostingCategoryEnum.None)
                {
                    using (SqlConnection conn = new SqlConnection(conn_string))
                    {
                        conn.Open();
                        _convCosting = QuotationRepository.FetchCC_V(conn, _quotation);
                        conn.Close();
                    }
                }
                return _convCosting.DefaultView;
            }
        }
        #endregion Properties

        #region MF Costing For Reference
        public bool CanShowMFCosting
        {
            get
            {
                _canShowMFCosting = true;
                return _canShowMFCosting;
            }
        }
        public bool ShowMFCosting
        {
            get { return _showMFCosting; }
            set { _showMFCosting = value; }
        }
        public DataTable MFConvCosting
        {
            get 
            {
                if (SelectedItem != null && CostingCategory != CostingCategoryEnum.None)
                {
                    using (SqlConnection conn = new SqlConnection(conn_string))
                    {
                        conn.Open();
                        Quotation temp = QuotationRepository.FetchQuotation(conn, SelectedItem, -1);
                        _mfConvCosting = QuotationRepository.FetchCC_V(conn, temp);
                        conn.Close();
                    }
                }
                return _mfConvCosting; 
            }
        }
        public RMCosting MFRMCosting
        {
            get 
            {
                if (SelectedItem != null && CostingCategory != CostingCategoryEnum.None)
                {
                    using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                    {
                        conn.Open();
                        Quotation temp = QuotationRepository.FetchQuotation(conn, SelectedItem, -1);
                        _mfRMCosting = QuotationRepository.FetchRM_V(conn, temp);
                        conn.Close();

                    }
                }
                return _mfRMCosting; 
            }
        }

        #endregion MF Costing For Reference

        #region Commands

        public ICommand SaveDraftCommand
        {
            get
            {
                if(_saveDraftCommand == null)
                {
                    _saveDraftCommand = new Command(p => SaveDraft(), p => CanSaveDraft());
                }
                return _saveDraftCommand;
            }
        }

        public ICommand SaveVersionCommand
        {
            get
            {
                if(_saveVersionCommand == null)
                {
                    _saveVersionCommand = new Command(p => SaveVersion(), p => CanSaveVersion());
                }
                return _saveVersionCommand;
            }
        }

        public ICommand SelectItemCommand
        {
            get
            {
                if (_selectItemCommand == null)
                {
                    _selectItemCommand = new Command(p =>
                    {
                        new PopupWindowViewModel().Show(new SelectItemViewModel());
                    });
                }
                return _selectItemCommand;
            }
        }
        public ICommand ShowMFCostingCommand
        {
            get
            {
                if (_showMFCostingCommand == null)
                {
                    _showMFCostingCommand = new Command(p => 
                    {
                        if (!_showMFCosting)
                        {
                            _showMFCosting = true;
                            OnPropertyChanged(nameof(ShowMFCosting));
                            OnPropertyChanged(nameof(MFConvCosting));
                            OnPropertyChanged(nameof(MFRMCosting));
                        }
                        else
                        {
                            _showMFCosting = false;
                            OnPropertyChanged(nameof(ShowMFCosting));
                        }
                    });
                }
                return _showMFCostingCommand;
            }
        }

        #endregion Commands

        #region Constructor
        public CostingViewModel()
        {
            SelectedItem = null;
            _rmCosting = new RMCosting() { RMConsidered = new RM()};
            _rmCosting.PropertyChanged += RMCosting_PropertyChanged;
        }

        #endregion Constructor

        #region Methods
        private void RMCosting_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RMCosting));
        }

        private int GetVersionNumber()
        {
            if (CostingCategory == CostingCategoryEnum.Metaforge)
            {
                return -1;
            }
            else if (CostingCategory == CostingCategoryEnum.None)
            {
                return -2;
            }
            else
            {
                int versionNumber;
                using (SqlConnection conn = new SqlConnection(conn_string))
                {
                    conn.Open();
                    versionNumber = QuotationRepository.FetchVersionNumber(conn, SelectedItem);
                    conn.Close();
                }
                return Math.Max(0, versionNumber); ;
            }
        }
        private Quotation GetQuotation()
        {
            Quotation quotation;
            using(SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                quotation = QuotationRepository.FetchQuotation(conn, SelectedItem, _versionNumber);
                conn.Close();
            }
            return quotation;
        }
        private RMCosting GetRMCosting( RMCosting rmCosting)
        {
            if (SelectedItem != null && CostingCategory != CostingCategoryEnum.None)
            {
                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conn_string))
                {
                    conn.Open();
                    rmCosting = QuotationRepository.FetchRM_V(conn, _quotation);
                    conn.Close();
                }
            }
            return rmCosting;
        }
        #endregion Methods

        #region Command Methods
        private void SaveDraft()
        {
            // Init the properties before inserting them in the database
            if (CostingCategory == CostingCategoryEnum.Metaforge) { Quotation.V_Number = -1; }
            else if (CostingCategory == CostingCategoryEnum.Customer) { Quotation.V_Number = 0; }
            Quotation.Q_Number = Quotation.GetQ_Number(SelectedItem.Id);
            RMCosting.CurrentRMRate = RMCosting.RMConsidered.CurrentRate;

            using(SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                try
                {
                    QuotationRepository.Upsert(conn, SelectedItem, Quotation, _convCosting, RMCosting);   
                }
                finally { conn.Close(); }
            }
        }

        private void SaveVersion()
        {
            Quotation.V_Number += 1;
            Quotation.Q_Number = Quotation.GetQ_Number(SelectedItem.Id);
            RMCosting.CurrentRMRate = RMCosting.RMConsidered.CurrentRate;

            using (SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                try
                {
                    QuotationRepository.Insert(conn, SelectedItem, Quotation, _convCosting, RMCosting);
                }
                finally { conn.Close(); }
            }
        }
        private bool CanSaveDraft()
        {
            if (Quotation.V_Number <= 0 && ( CostingCategory == CostingCategoryEnum.Metaforge || CostingCategory == CostingCategoryEnum.Customer ) ) { return true; }
            else { return false; }
        }

        private bool CanSaveVersion()
        {
            if (CostingCategory == CostingCategoryEnum.Metaforge) { return false; } 
            else { return true; }
        }
        #endregion Command Methods
    }
}

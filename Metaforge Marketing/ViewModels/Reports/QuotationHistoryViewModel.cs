using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.Repository;
using Metaforge_Marketing.ViewModels.Shared;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Reports
{
    public class QuotationHistoryViewModel : SharedViewModelBase
    {
        #region Fields
        private readonly string conn_string = Properties.Settings.Default.conn_string;
        private int _selectedVersion = -2;
        private ObservableCollection<int> _versions;
        private ICommand _selectItemCommand;
        private Quotation _quotation;
        #endregion Fields

        public QuotationHistoryViewModel() 
        {
            ClearAllSelections();
            StaticPropertyChanged += QuotationHistoryViewModel_StaticPropertyChanged;
        }

        #region Commands
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
        #endregion Commands

        #region Properties

        public Quotation Quotation
        {
            get { return _quotation; }
        }
        public int SelectedVersion 
        { 
            get { return _selectedVersion; } 
            set 
            { 
                if(_selectedVersion != value && SelectedItem != null)
                {
                    _selectedVersion = value;
                    _quotation = GetQuotation(SelectedItem, _selectedVersion);
                    OnPropertyChanged(nameof(Quotation));
                }
            } 
        }
        public ObservableCollection<int> Versions { get { return _versions; } }

        #endregion Properties

        #region Methods
        private Quotation GetQuotation(Item item, int versionNumber)
        {
            Quotation quotation;
            using(SqlConnection conn = new SqlConnection(conn_string))
            {
                conn.Open();
                quotation = QuotationRepository.FetchQuotation(conn, item, versionNumber);
                quotation.RMCosting = QuotationRepository.FetchRM_V(conn, quotation);
                quotation.ConvCosting = QuotationRepository.FetchConvCosting(conn, quotation);
                conn.Close();
            }
            return quotation;
        }
        private ObservableCollection<int> GetVersions()
        {
            ObservableCollection<int> versions = new ObservableCollection<int>();
            if (SelectedItem != null)
            {
                using (SqlConnection conn = new SqlConnection(conn_string))
                {
                    conn.Open();
                    versions = new ObservableCollection<int>(Repository.QuotationRepository.FetchVersions(conn, SelectedItem));
                    conn.Close();
                }
            }
            return versions;
        }

        private void QuotationHistoryViewModel_StaticPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedItem))
            {
                _versions = GetVersions();
                OnPropertyChanged(nameof(Versions));
            }
        }
        #endregion Methods
    }
}



using Metaforge_Marketing.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Metaforge_Marketing.ViewModels.Shared
{
    public abstract class SharedViewModelBase :ViewModelBase
    {
        #region INPC Static
        public static event PropertyChangedEventHandler StaticPropertyChanged;

        public static void NotifyStaticPropertyChanged([CallerMemberName] string name = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(name));
        }
        #endregion INPC Static

        #region Fields
        private static Item _selectedItem;
        private static RFQ _selectedRFQ;
        private static Customer _selectedCustomer;
        private static Buyer _selectedBuyer;
        #endregion Fields

        #region Properties
        public static Item SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem!= value)
                {
                    _selectedItem = value;
                    NotifyStaticPropertyChanged(nameof(SelectedItem));
                }
            }
        }

        public static RFQ SelectedRFQ
        {
            get { return _selectedRFQ; }
            set
            {
                if (_selectedRFQ!= value)
                {
                    _selectedRFQ = value;
                    NotifyStaticPropertyChanged(nameof(SelectedRFQ));
                }
            }
        }

        public static Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                if (_selectedCustomer!= value)
                {
                    _selectedCustomer = value;
                    NotifyStaticPropertyChanged(nameof(SelectedCustomer));
                }
            }
        }

        public static Buyer SelectedBuyer
        {
            get { return _selectedBuyer; }
            set
            {
                if (_selectedBuyer != value)
                {
                    _selectedBuyer = value;
                    NotifyStaticPropertyChanged(nameof(SelectedBuyer));
                }
            }
        }
        #endregion Properties

        #region Methods
        public void ClearAllSelections()
        {
            SelectedCustomer = null;
            SelectedItem = null;
            SelectedRFQ = null;
            SelectedBuyer = null;
        }
        #endregion Methods
    }
}

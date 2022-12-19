

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
        private Item _selectedItem;
        private RFQ _selectedRFQ;
        #endregion Fields

        #region Properties
        public Item SelectedItem
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

        public RFQ SelectedRFQ
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
        #endregion Properties
    }
}


using Metaforge_Marketing.Models;

namespace Metaforge_Marketing.ViewModels.RFQs.POViewModels
{
    public class POContainer : ViewModelBase
    {
        private static Item _selectedItem;

        public static Item SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; }
        }
    }
}

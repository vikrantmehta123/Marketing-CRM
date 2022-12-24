using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.RFQs
{
    public class UpdateItemStatusViewModel : ViewModelBase
    {
        #region Fields
        private bool _isAddingPODetails;
        private ICommand _saveCommand, _clearCommand;
        #endregion Fields

        #region Properties
        public Item SelectedItem { get; private set; }
        public ICommand AddPODetails
        {
            get
            {
                return new Command(p => {
                    if (_isAddingPODetails) { _isAddingPODetails = false; }
                    else { _isAddingPODetails = true; }
                    OnPropertyChanged(nameof(IsAddingPODetails));
                });
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand;
            }
        }

        public ICommand ClearCommand
        {
            get { return _clearCommand; }
        }
        public bool IsAddingPODetails
        {
            get 
            { 
                return _isAddingPODetails; 
            }
        }
        #endregion Properties

        public UpdateItemStatusViewModel( Item selectedItem) 
        {
            SelectedItem= selectedItem;
        }
    }
}

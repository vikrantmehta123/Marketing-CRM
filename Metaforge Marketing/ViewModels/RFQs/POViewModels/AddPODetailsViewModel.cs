

using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.ViewModels.Shared;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.RFQs.POViewModels
{
    public class AddPODetailsViewModel : ViewModelBase
    {
        #region Fields
        private ICommand _selectItemCommand, _saveCommand;
        private PO _poToAdd = new PO();
        #endregion Fields

        #region Properties
        public PO POToAdd
        {
            get { return _poToAdd; }
            set { _poToAdd = value; }
        }

        public ICommand SelectItemCommand
        {
            get
            {
                if (_selectItemCommand == null)
                {
                    _selectItemCommand = new Command(p => new PopupWindowViewModel().Show(new POViewModels.SelectPOItemViewModel()));
                }
                return _selectItemCommand;
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand== null)
                {
                    _saveCommand = new Command(p => Save(), p => CanSave());
                }
                return _saveCommand;
            }
        }
        #endregion Properties

        #region Methods

        private void Save()
        {
            // Insert into PO table
            // Update ItemHistory
            // Update Item status
        }

        private bool CanSave()
        {
            return POToAdd.IsFormDataValid();
        }
        #endregion Methods
    }
}

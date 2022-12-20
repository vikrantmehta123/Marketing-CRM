
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Add
{
    public class AddBuyerViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<Buyer> _buyers;
        private Buyer _buyerToAdd;
        private ICommand _addAnotherBuyerCommand, _clearCommand;

        #endregion Fields

        #region Bounded Properties
        public Buyer BuyerToAdd { get { return _buyerToAdd; } set { _buyerToAdd = value; } }
        public ICommand AddAnotherBuyerCommand
        {
            get
            {
                if (_addAnotherBuyerCommand == null)
                {
                    _addAnotherBuyerCommand = new Command(p => AddAnotherBuyer(), p => CanAddAnotherBuyer());
                }
                return _addAnotherBuyerCommand;
            }
        }
        public ICommand ClearCommand
        {
            get
            {
                if(_clearCommand == null)
                {
                    _clearCommand = new Command(p => Clear());
                }
                return _clearCommand;
            }
        }
        #endregion Bounded Properties

        public AddBuyerViewModel()
        {
            BuyerToAdd = new Buyer();
        }

        #region Command Functions
        private void AddAnotherBuyer()
        {

        }
        private bool CanAddAnotherBuyer()
        {
            return true;
        }
        private void Save()
        {

        }

        private void Clear()
        {

        }
        #endregion Command Functions

    }
}

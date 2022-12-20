using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.ViewModels.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Costing
{
    public class MFCostingViewModel : SharedViewModelBase
    {
        #region Fields
        private Models.Costing _costing = new Models.Costing();
        private RMCosting _mfRMCosting = new RMCosting();
        private List<Operation> _mfConversionCostingList = new List<Operation>();
        private ICommand _saveCommand, _clearCommand, _selectItemCommand;
        private bool _showDetailedCosting, _showShortCosting;
        #endregion Fields


        #region Properties

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
        public RMCosting MFRMCosting { get { return _mfRMCosting; } set { _mfRMCosting = value; } }
        public List<Operation> MFCC { get { return _mfConversionCostingList; } set { _mfConversionCostingList = value; } }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new Command(p => Save());
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


        #region Methods
        private void Save()
        {

        }

        private void Clear()
        {
            
        }
        #endregion Methods
    }
}

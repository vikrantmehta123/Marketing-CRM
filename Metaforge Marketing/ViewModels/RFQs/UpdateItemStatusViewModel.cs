using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using System;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.RFQs
{
    public class UpdateItemStatusViewModel : ViewModelBase
    {
        #region Fields
        private bool _canAllowMoreChanges = true;
        private bool _isRegretted, _isRejected;
        private bool _isAddingPODetails;
        private DateTime _dateOfEvent = DateTime.Today;
        private string _note;
        private ICommand _saveCommand, _clearCommand;
        #endregion Fields

        #region Properties
        public bool CanAllowMoreChanges
        {
            get { return _canAllowMoreChanges;}
            private set { _canAllowMoreChanges = value;}
        }
        public bool IsRegretted
        {
            get { return _isRegretted; }
            set { 
                if (value) { _canAllowMoreChanges = false; }
                else { _canAllowMoreChanges = true; }
                OnPropertyChanged(nameof(CanAllowMoreChanges));
                _isRegretted = value; 
            }
        }
        public bool IsRejected
        {
            get { return _isRejected; }
            set
            {
                if (value) { _canAllowMoreChanges = false; }
                else { _canAllowMoreChanges = true; }
                _isRejected = value;
            }
        }

        public DateTime DateOfEvent { get { return _dateOfEvent; } set { _dateOfEvent = value; } }
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

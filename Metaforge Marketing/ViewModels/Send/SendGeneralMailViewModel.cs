
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.ViewModels.Shared;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Send
{
    public class SendGeneralMailViewModel : ViewModelBase
    {
        #region Fields
        private ICommand _selectRecipientsCommand, _sendCommand, _clearCommand;
        #endregion Fields

        #region Properties
        public Email Email { get; set; }
        public ICommand SelectRecipientsCommand
        {
            get
            {
                if (_selectRecipientsCommand == null)
                {
                   // _selectRecipientsCommand = new Command(p => new PopupWindowViewModel().Show(new SelectRecipientsViewModel()));
                }
                return _selectRecipientsCommand;
            }
        }

        public ICommand SendCommand
        {
            get
            {
                if (_sendCommand== null)
                {
                    _sendCommand = new Command(p => 
                    {
                        Email.Send();
                    });
                }
                return _sendCommand;
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                {
                    _clearCommand = new Command(p => 
                    {
                        Email = new Email();
                        OnPropertyChanged(nameof(Email));
                    });
                }
                return _clearCommand;
            }
        }
        #endregion Properties
    }
}

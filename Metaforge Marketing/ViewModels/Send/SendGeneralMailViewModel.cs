
using Metaforge_Marketing.HelperClasses;
using Metaforge_Marketing.Models;
using Metaforge_Marketing.ViewModels.Shared;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Send
{
    public class SendGeneralMailViewModel : GeneralMailContainer
    {
        #region Fields
        private ICommand _selectRecipientsCommand, _sendCommand, _clearCommand;
        private Email _email = new Email();
        #endregion Fields

        #region Properties
        public Email Email
        {
            get { return _email; }
            set 
            {
                if(_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }
        public int RecipientsCount
        {
            get { return Recipients.Distinct().Count(); }
        }
        public ICommand SelectRecipientsCommand
        {
            get
            {
                if (_selectRecipientsCommand == null)
                {
                    _selectRecipientsCommand = new Command(p => new PopupWindowViewModel().Show(new SelectRecipientsViewModel()));
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

        public SendGeneralMailViewModel()
        {
            VisitedRecipients = null;
            Email.PropertyChanged += Email_PropertyChanged;
        }
        private void Email_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Email.MailMessage.Body))
            {
                OnPropertyChanged(nameof(Email));
            }
        }
    }
}

using System.ComponentModel;


namespace Metaforge_Marketing.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INPC

        private ViewModelBase _currentPageViewModel;

        public ViewModelBase CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged(nameof(CurrentPageViewModel));
                }
            }
        }
        public void ChangeViewModel(ViewModelBase viewModel)
        {
            CurrentPageViewModel = viewModel;
        }
    }
}

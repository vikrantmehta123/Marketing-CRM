using System.Windows;

namespace Metaforge_Marketing.ViewModels.Shared
{
    public class PopupWindowViewModel : SharedViewModelBase
    {
        public void Show(ViewModelBase viewModel)
        {
            CurrentPageViewModel = viewModel;
            Window PopupWindow = new PopupWindow();
            PopupWindow.Content = CurrentPageViewModel;
            PopupWindow.ShowDialog();
        }
    }
}

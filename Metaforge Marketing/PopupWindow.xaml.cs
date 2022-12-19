using System.Windows;


namespace Metaforge_Marketing
{
    public partial class PopupWindow : Window
    {
        public PopupWindow()
        {
            DataContext = new ViewModels.Shared.PopupWindowViewModel();
            InitializeComponent();
        }
    }
}

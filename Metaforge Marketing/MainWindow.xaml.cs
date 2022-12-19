using System.Windows;


namespace Metaforge_Marketing
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new ViewModels.MainWindowViewModel();   
            InitializeComponent();
        }
    }
}

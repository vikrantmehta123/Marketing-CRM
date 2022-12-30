using System.Windows.Controls;

namespace Metaforge_Marketing.Views.Add
{
    public partial class AddCustomerView : UserControl
    {
        public AddCustomerView()
        {
            DataContext = new ViewModels.Add.AddCustomerViewModel();
            InitializeComponent();
        }

        private void AddBuyer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AddBuyerForm.Visibility = System.Windows.Visibility.Visible;
            MainButtonPanel.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void HideBuyer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AddBuyerForm.Visibility = System.Windows.Visibility.Collapsed;
            MainButtonPanel.Visibility= System.Windows.Visibility.Visible;
        }

    }
}

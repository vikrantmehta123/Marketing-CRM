using System.Windows.Controls;

namespace Metaforge_Marketing.Views.Test
{
    public partial class TestCostingView : UserControl
    {
        public TestCostingView()
        {
            DataContext = new ViewModels.Test.TestCostingViewModel();
            InitializeComponent();
        }
    }
}

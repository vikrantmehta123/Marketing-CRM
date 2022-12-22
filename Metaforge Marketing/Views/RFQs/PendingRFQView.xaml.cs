using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Metaforge_Marketing.Views.RFQs
{
    public partial class PendingRFQView : UserControl
    {
        public PendingRFQView()
        {
            DataContext = new ViewModels.RFQs.PendingRFQViewModel();
            InitializeComponent();
        }
    }
}

using System.Windows.Controls;


namespace Metaforge_Marketing.Views.Mails
{
    public partial class GeneralMailView : UserControl
    {
        public GeneralMailView()
        {
            DataContext = new ViewModels.Send.SendGeneralMailViewModel();
            InitializeComponent();
        }
    }
}

using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Metaforge_Marketing.ViewModels.Shared
{
    public abstract class PopupCloseMarker : PopupWindowViewModel
    {
        /// <summary>
        /// Represents the class for the viewmodels that are supposed to open in Popup window such as Select Item
        /// </summary>
        public abstract ICommand SelectionDoneCommand { get; }

        public abstract void ClearSelection();

        public void Close(object o)
        {
            Window.GetWindow(((UserControl)o)).DialogResult = true;
        }
        public abstract bool IsSelectionDone();
    }
}

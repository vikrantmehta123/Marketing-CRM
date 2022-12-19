using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Shared
{
    public class SelectItemViewModel : PopupCloseMarker
    {
        public override ICommand SelectionDoneCommand => throw new System.NotImplementedException();

        public override void ClearSelection()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsSelectionDone()
        {
            throw new System.NotImplementedException();
        }
    }
}

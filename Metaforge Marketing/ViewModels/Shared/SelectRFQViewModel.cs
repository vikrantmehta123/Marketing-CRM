using System;
using System.Windows.Input;

namespace Metaforge_Marketing.ViewModels.Shared
{
    internal class SelectRFQViewModel : PopupCloseMarker
    {
        public override ICommand SelectionDoneCommand => throw new NotImplementedException();

        public override void ClearSelection()
        {
            throw new NotImplementedException();
        }

        public override bool IsSelectionDone()
        {
            throw new NotImplementedException();
        }
    }
}

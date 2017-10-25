using Windows.ApplicationModel.DataTransfer;
using kmd.Core.Hotkeys;

namespace kmd.Core.Helpers
{
    public class DragOperations
    {
        public static DataPackageOperation UserRequestedDragOperation =>
            KeyEventsAgregator.IsCtrlKeyPressed ? DataPackageOperation.Copy : DataPackageOperation.Move;

        public static DataPackageOperation AvailableDragOperations => DataPackageOperation.Copy | DataPackageOperation.Move;
    }
}
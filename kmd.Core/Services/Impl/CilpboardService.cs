using kmd.Core.Services.Contracts;
using Windows.ApplicationModel.DataTransfer;

namespace kmd.Core.Services.Impl
{
    public class CilpboardService : ICilpboardService
    {
        public DataPackageView Get()
        {
            return Clipboard.GetContent();
        }

        public void Set(DataPackage dataObject)
        {
            Clipboard.SetContent(dataObject);
        }
    }
}
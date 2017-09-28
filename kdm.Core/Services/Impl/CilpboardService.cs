using kdm.Core.Services.Contracts;
using Windows.ApplicationModel.DataTransfer;

namespace kdm.Core.Services.Impl
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
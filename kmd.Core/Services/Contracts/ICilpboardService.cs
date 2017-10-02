using Windows.ApplicationModel.DataTransfer;

namespace kmd.Core.Services.Contracts
{
    public interface ICilpboardService
    {
        DataPackageView Get();

        void Set(DataPackage dataObject);
    }
}
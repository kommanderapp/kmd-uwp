using Windows.ApplicationModel.DataTransfer;

namespace kdm.Core.Services.Contracts
{
    public interface ICilpboardService
    {
        void Set(DataPackage dataObject);

        DataPackageView Get();
    }
}
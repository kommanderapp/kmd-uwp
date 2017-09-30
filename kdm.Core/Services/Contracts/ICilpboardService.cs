using Windows.ApplicationModel.DataTransfer;

namespace kdm.Core.Services.Contracts
{
    public interface ICilpboardService
    {
        DataPackageView Get();

        void Set(DataPackage dataObject);
    }
}
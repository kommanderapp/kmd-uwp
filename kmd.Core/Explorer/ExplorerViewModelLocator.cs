using Microsoft.Practices.ServiceLocation;

namespace kmd.Core.Explorer
{
    public class ExplorerViewModelLocator
    {
        public ExplorerViewModel ExplorerViewModel => ServiceLocator.Current.GetInstance<ExplorerViewModel>();
    }
}
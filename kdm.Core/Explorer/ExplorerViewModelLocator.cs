using kmd.Core.Explorer;
using Microsoft.Practices.ServiceLocation;

namespace kdm.Core.Explorer
{
    public class ExplorerViewModelLocator
    {
        public ExplorerViewModel ExplorerViewModel => ServiceLocator.Current.GetInstance<ExplorerViewModel>();
    }
}
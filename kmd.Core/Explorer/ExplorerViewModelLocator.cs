using CommonServiceLocator;

namespace kmd.Core.Explorer
{
    public class ExplorerViewModelLocator
    {
        public ExplorerViewModel ExplorerViewModel => ServiceLocator.Current.GetInstance<ExplorerViewModel>();
    }
}

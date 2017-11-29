using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmd.Core.Comparer
{
    public class ComparerViewModelLocator
    {
        public ComparerViewModel ComparerViewModel => ServiceLocator.Current.GetInstance<ComparerViewModel>();
    }
}

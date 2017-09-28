using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;

namespace kdm.Core.Interactivity
{
    public class InvokeCommandBaseAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
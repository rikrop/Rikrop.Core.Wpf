using System;
using System.Linq.Expressions;

namespace Rikrop.Core.Wpf
{
    public interface ILinkedPropertyChanged
    {
        ILinkedPropertyChanged Notify(Expression<Func<object>> targetProperty);
        ILinkedPropertyChanged Execute(Action action);
    }
}
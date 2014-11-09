using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Rikrop.Core.Wpf
{
    public interface ILinkedObjectChanged
    {
        ILinkedObjectChanged AfterNotify(Expression<Func<object>> sourceProperty);

        ILinkedObjectChanged AfterNotify<T>(T sourceChangeNotifier, Expression<Func<T, object>> sourceProperty)
            where T : INotifyPropertyChanged;

        ILinkedObjectChanged BeforeNotify(Expression<Func<object>> sourceProperty);

        ILinkedObjectChanged BeforeNotify<T>(T sourceChangeNotifier, Expression<Func<T, object>> sourceProperty)
            where T : ChangeNotifier;
    }
}
using System.Windows;
using System.Windows.Data;

namespace Rikrop.Core.Wpf
{
    public static class BindingTools
    {
        public static void SetBinding(object source, string sourcePropertyPath, DependencyObject target,
                                      DependencyProperty targetProperty, BindingMode bindingMode = BindingMode.Default, IValueConverter converter = null)
        {
            var binding = new Binding {Path = new PropertyPath(sourcePropertyPath), Source = source, Mode = bindingMode, Converter = converter};
            BindingOperations.SetBinding(target, targetProperty, binding);
        }

        public static void SetBinding(object source, DependencyProperty sourceProperty, DependencyObject target,
                                      DependencyProperty targetProperty, BindingMode bindingMode = BindingMode.Default, IValueConverter converter = null)
        {
            var binding = new Binding { Path = new PropertyPath(sourceProperty), Source = source, Mode = bindingMode, Converter = converter };
            BindingOperations.SetBinding(target, targetProperty, binding);
        }

        public static Binding GetBinding(object source, string sourcePropertyPath, BindingMode bindingMode = BindingMode.Default, IValueConverter converter = null)
        {
            var binding = new Binding {Path = new PropertyPath(sourcePropertyPath), Source = source, Mode = bindingMode, Converter = converter};
            return binding;
        }
    }
}
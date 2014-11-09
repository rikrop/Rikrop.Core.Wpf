using System.Diagnostics;
using System.Windows;

namespace Rikrop.Core.Wpf.Mvvm
{
    public class ViewModel : ChangeNotifier, IViewModel
    {
        private FrameworkElement _content;
        public virtual FrameworkElement Content
        {
            get { return _content; }
            set
            {
                if (!Equals(_content, value))
                {
                    _content = value;
                    if (_content != null)
                    {
                        _content.DataContext = this;
                    }
                    NotifyPropertyChanged(() => Content);
                }
            }
        }

        ~ViewModel()
        {
            Debug.WriteLine("'{0}'({1}) was disposed.", GetType().Name, GetHashCode());
        }
    }

    public abstract class ViewModel<TView> : ViewModel
        where TView : FrameworkElement, new()
    {
        public TView TypedContent
        {
            get { return (TView)Content; }
        }

        public override FrameworkElement Content
        {
            get { return base.Content ?? (base.Content = new TView()); }
            set { base.Content = value; }
        }
    }
}
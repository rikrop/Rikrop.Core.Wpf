using Rikrop.Core.Wpf.Mvvm;

namespace Rikrop.Core.Wpf.Test.Mvvm.Navigation
{
    public class NavigationFakeWorkspace : Wpf.Mvvm.Workspace
    {
        private readonly string _displayName;

        public override string DisplayName
        {
            get { return _displayName; }
        }

        public NavigationFakeWorkspace()
            : this(typeof(NavigationFakeWorkspace).FullName)
        {
            
        }

        public NavigationFakeWorkspace(string displayName)
        {
            _displayName = displayName;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
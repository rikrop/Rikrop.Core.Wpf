using Rikrop.Core.Wpf.Mvvm;

namespace Rikrop.Core.Wpf.Async
{
    public class BusyPopupWorkspace : Workspace<BusyPopupView>
    {
        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public override string DisplayName
        {
            get { return Title; }
        }

        public BusyPopupWorkspace()
        {
            Title = "Пожалуйста, подождите...";
        }
    }
}
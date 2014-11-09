using System.Windows;

namespace Rikrop.Core.Wpf.Controls.ErrorReport
{
    public partial class ErrorReportWindow : Window
    {
        public ErrorReportWindow()
        {
            InitializeComponent();
            OldHeight = Height;
        }

        private double OldHeight { get; set; }

        private void expander_Expanded(object sender, RoutedEventArgs e)
        {
            Height = OldHeight;
            MinHeight = 340;
        }

        private void expander_Collapsed(object sender, RoutedEventArgs e)
        {
            OldHeight = Height;
            Height = MinHeight = 170;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == DataContextProperty)
            {
                var workspace = DataContext as ErrorReportWorkspace;
                if (workspace != null)
                {
                    workspace.RequestClose += (sender, args) => Close();
                }
            }
            base.OnPropertyChanged(e);
        }
    }
}

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Rikrop.Core.Wpf.Controls.Validation.VGrid
{
    public class ValidatedRowDefinition : RowDefinition
    {
        public ValidatedRowDefinition()
        {
            ValidationErrorBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xFA, 0xCb, 0xD1));
        }

        public int ValidationElementStartColumn { get; set; }
        public int ValidationColumnSpan { get; set; }
        public Brush ValidationErrorBrush { get; set; }
        public Binding ValidationBinding { get; set; }
    }
}
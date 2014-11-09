using System;
using System.Collections;

namespace Rikrop.Core.Wpf.Controls.DataGrid
{
    public class RrcDataGridItemsSourceChangedEventArgs : EventArgs
    {
        public IEnumerable OldValue { get; set; }
        public IEnumerable NewValue { get; set; }
    }
}

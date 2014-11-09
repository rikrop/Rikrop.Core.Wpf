using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Windows.Data;
using Rikrop.Core.Framework;

namespace Rikrop.Core.Wpf.Converters
{
    public class DateRangeToStringConverter : IValueConverter
    {
        private string _nullDateText = @"н\д";

        public bool ShowTime { get; set; }

        public string NullDateText
        {
            get { return _nullDateText; }
            set { _nullDateText = value; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.Assume(value != null);
            Contract.Assume(value is DateRange);

            var format = ShowTime
                             ? "dd.MM.yyyy HH:mm:ss"
                             : "dd.MM.yyyy";

            var dateRange = value as DateRange;
            // конечную дату отображаем днем раньше т.к. пользователь думает что последний день включен в отображаемый диапазон
            return dateRange.Start.ConvertTo(a => a.ToString(format), _nullDateText) + " - " + dateRange.End.ConvertTo(a => a.AddDays(-1).ToString(format), _nullDateText);
//            return dateRange.Start.Value.ToString(format) + " - " + dateRange.End.Value.AddDays(-1).ToString(format); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
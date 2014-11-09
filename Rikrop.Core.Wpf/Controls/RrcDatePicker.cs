using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Rikrop.Core.Wpf.Controls
{
    public class RrcDatePicker : DatePicker
    {
        public static readonly DependencyProperty MinDateProperty = DependencyProperty.Register(
            "MinDate",
            typeof(DateTime?),
            typeof(RrcDatePicker),
            new PropertyMetadata(null, MinDateCallback));

        public static readonly DependencyProperty MaxDateProperty = DependencyProperty.Register(
            "MaxDate",
            typeof(DateTime?),
            typeof(RrcDatePicker),
            new PropertyMetadata(null, MaxDateCallback));

        public DateTime? MinDate
        {
            get { return (DateTime?)GetValue(MinDateProperty); }
            set { SetValue(MinDateProperty, value); }
        }

        public DateTime? MaxDate
        {
            get { return (DateTime?)GetValue(MaxDateProperty); }
            set { SetValue(MaxDateProperty, value); }
        }        

        private static void MinDateCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var datePicker = (RrcDatePicker)dependencyObject;
            var oldValue = (DateTime?)args.OldValue;
            var newValue = (DateTime?)args.NewValue;

            datePicker.OnMinDateChanged(newValue, oldValue);
        }

        protected virtual void OnMinDateChanged(DateTime? newValue, DateTime? oldValue)
        {
            CorrectBlackoutDates(
                            this,
                            oldValue,
                            newValue,
                            date => DateTime.MinValue,
                            date => date == DateTime.MinValue ? date : date.AddDays(-1));
            DisplayDateStart = newValue;
        }

        private static void MaxDateCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var datePicker = (RrcDatePicker)dependencyObject;
            var oldValue = (DateTime?)args.OldValue;
            var newValue = (DateTime?)args.NewValue;

            datePicker.OnMaxDateChanged(newValue,oldValue);
        }

        protected virtual void OnMaxDateChanged(DateTime? newValue,DateTime? oldValue)
        {
            CorrectBlackoutDates(
                this,
                oldValue,
                newValue,
                date => date == DateTime.MaxValue ? date : date.AddDays(1),
                date => DateTime.MaxValue);
            DisplayDateEnd = newValue;
        }

        private static void CorrectBlackoutDates(
            RrcDatePicker picker,
            DateTime? oldValue,
            DateTime? newValue,
            Func<DateTime, DateTime> startBlackoutDateFunc,
            Func<DateTime, DateTime> endBlackoutDateFunc)
        {
            if (oldValue != null)
            {
                RemoveRange(
                    picker.BlackoutDates,
                    r => r.Start == startBlackoutDateFunc(oldValue.Value) &&
                         r.End == endBlackoutDateFunc(oldValue.Value));
            }


            if (newValue != null)
            {
                var newRange = new CalendarDateRange(
                    startBlackoutDateFunc(newValue.Value),
                    endBlackoutDateFunc(newValue.Value));
                if (picker.SelectedDate != null && RangeContainsDate(newRange, picker.SelectedDate.Value))
                {
                    picker.SelectedDate = newValue.Value;
                }
                picker.BlackoutDates.Add(
                    new CalendarDateRange(
                        startBlackoutDateFunc(newValue.Value),
                        endBlackoutDateFunc(newValue.Value)));
            }
        }

        private static bool RangeContainsDate(CalendarDateRange range, DateTime date)
        {
            return range.Start <= date && date <= range.End;
        }

        private static void RemoveRange(ICollection<CalendarDateRange> collection,
                                        Predicate<CalendarDateRange> predicate)
        {
            var oldRange = collection.FirstOrDefault(range => predicate(range));
            if (oldRange != null)
            {
                collection.Remove(oldRange);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            DatePickerTextBox datePickerTextBox = GetTemplateChild("PART_TextBox") as DatePickerTextBox;

            if (datePickerTextBox == null)
                return;

            datePickerTextBox.Loaded += new RoutedEventHandler(datePickerTextBox_Loaded);
        }

        void datePickerTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var datePickerTextBox = sender as DatePickerTextBox;
            if (datePickerTextBox == null)
                return;

            datePickerTextBox.Loaded -= datePickerTextBox_Loaded;

            var partWatermark = datePickerTextBox.Template.FindName("PART_Watermark", datePickerTextBox) as ContentControl;
            if (partWatermark == null)
                return;

            partWatermark.Content = "Выберите дату";
            var brush = new SolidColorBrush {Color = Colors.LightGray};
            partWatermark.Foreground = brush;
        }
    }
}

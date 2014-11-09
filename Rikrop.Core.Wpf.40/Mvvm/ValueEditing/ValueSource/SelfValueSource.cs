namespace Rikrop.Core.Wpf.Mvvm.ValueEditing.ValueSource
{
    public class SelfValueSource<TValue> : ChangeNotifier, IValueSource<TValue>
    {
        private TValue _value;

        public TValue Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        public SelfValueSource(TValue initialValue)
        {
            Value = initialValue;
        }
    }
}
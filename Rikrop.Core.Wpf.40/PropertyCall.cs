namespace Rikrop.Core.Wpf
{
    internal class PropertyCall
    {
        private readonly object _targetObject;
        private readonly string _targetPropertyName;

        public PropertyCall(object targetObject, string targetPropertyName)
        {
            _targetObject = targetObject;
            _targetPropertyName = targetPropertyName;
        }

        public object TargetObject
        {
            get { return _targetObject; }
        }

        public string TargetPropertyName
        {
            get { return _targetPropertyName; }
        }
    }
}
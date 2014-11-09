using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Rikrop.Core.Framework;

namespace Rikrop.Core.Wpf
{
    [DataContract(IsReference = true)]
    [Serializable]
    public abstract class ChangeNotifier : INotifyPropertyChanged
    {
        private Dictionary<string, LinkedPropertyChangeNotifierListeners> _afterChangeLinkedChangeNotifierProperties;
        private Dictionary<string, LinkedPropertyStandardNotifierListeners> _afterChangeLinkedStandardNotifierProperties;
        private Dictionary<string, LinkedPropertyChangeNotifierListeners> _beforeChangeLinkedChangeNotifierProperties;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (Equals(field, value))
            {
                return;
            }

            field = value;
            NotifyPropertyChangedInternal(propertyName);
        }

        [IgnoreDataMember]
        private Dictionary<string, LinkedPropertyChangeNotifierListeners> AfterChangeLinkedChangeNotifierProperties
        {
            get
            {
                return _afterChangeLinkedChangeNotifierProperties ??
                       (_afterChangeLinkedChangeNotifierProperties =
                        new Dictionary<string, LinkedPropertyChangeNotifierListeners>());
            }
        }

        [IgnoreDataMember]
        private Dictionary<string, LinkedPropertyStandardNotifierListeners> AfterChangeLinkedStandardNotifierProperties
        {
            get
            {
                return _afterChangeLinkedStandardNotifierProperties ??
                       (_afterChangeLinkedStandardNotifierProperties =
                        new Dictionary<string, LinkedPropertyStandardNotifierListeners>());
            }
        }

        [IgnoreDataMember]
        private Dictionary<string, LinkedPropertyChangeNotifierListeners> BeforeChangeLinkedChangeNotifierProperties
        {
            get
            {
                return _beforeChangeLinkedChangeNotifierProperties ??
                       (_beforeChangeLinkedChangeNotifierProperties =
                        new Dictionary<string, LinkedPropertyChangeNotifierListeners>());
            }
        }

        private static void RegisterLinkedPropertyListener(string linkedPropertyName,
                                                           ChangeNotifier targetObject,
                                                           string targetPropertyName,
                                                           Dictionary<string, LinkedPropertyChangeNotifierListeners>
                                                               linkedProperties)
        {
            GetOrCreatePropertyListeners(linkedPropertyName, linkedProperties).Register(targetObject, targetPropertyName);
        }

        private static LinkedPropertyChangeNotifierListeners GetOrCreatePropertyListeners(string linkedPropertyName,
                                                                                  Dictionary<string, LinkedPropertyChangeNotifierListeners> linkedProperties)
        {
            LinkedPropertyChangeNotifierListeners changeNotifierListeners;
            if (!linkedProperties.TryGetValue(linkedPropertyName, out changeNotifierListeners))
            {
                changeNotifierListeners = new LinkedPropertyChangeNotifierListeners();
                linkedProperties.Add(linkedPropertyName, changeNotifierListeners);
            }
            return changeNotifierListeners;
        }

        private static void RegisterLinkedPropertyListener(string linkedPropertyName,
                                                           ChangeNotifier targetObject,
                                                           Action action,
                                                           Dictionary<string, LinkedPropertyChangeNotifierListeners>
                                                               linkedProperties)
        {
            GetOrCreatePropertyListeners(linkedPropertyName, linkedProperties).Register(targetObject, action);
        }

        protected void NotifyPropertyChanged(Expression<Func<object, object>> property)
        {
            NotifyPropertyChangedInternal((string) property.GetName());
        }

        protected void NotifyPropertyChanged(Expression<Func<object>> property)
        {
            NotifyPropertyChangedInternal((string) property.GetName());
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
        }

        protected ILinkedPropertyChanged AfterNotify(Expression<Func<object>> property)
        {
            var propertyCall = PropertyCallHelper.GetPropertyCall(property);
            return new AfterLinkedPropertyChanged((INotifyPropertyChanged) propertyCall.TargetObject,
                                                  propertyCall.TargetPropertyName,
                                                  this);
        }

        protected ILinkedPropertyChanged BeforeNotify(Expression<Func<object>> property)
        {
            var propertyCall = PropertyCallHelper.GetPropertyCall(property);
            return new BeforeLinkedPropertyChanged((ChangeNotifier) propertyCall.TargetObject,
                                                   propertyCall.TargetPropertyName,
                                                   this);
        }

        protected ILinkedPropertyChanged AfterNotify<T>(T changeNotifier, Expression<Func<T, object>> property)
            where T : INotifyPropertyChanged
        {
            return new AfterLinkedPropertyChanged(changeNotifier, property.GetName(), this);
        }

        protected ILinkedPropertyChanged BeforeNotify<T>(T changeNotifier, Expression<Func<T, object>> property)
            where T : ChangeNotifier
        {
            return new BeforeLinkedPropertyChanged(changeNotifier, property.GetName(), this);
        }

        protected ILinkedObjectChanged Notify(Expression<Func<object>> property)
        {
            return new LinkedObjectChanged(this, property.GetName());
        }

        private void NotifyPropertyChangedInternal(string propertyName)
        {
            var handler = PropertyChanged;
            NotifyPropertyChanged(handler, propertyName);
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            NotifyPropertyChanged(handler, propertyName);
        }

        private void NotifyPropertyChanged(PropertyChangedEventHandler handler, string propertyName)
        {
            NotifyLinkedPropertyListeners(propertyName, BeforeChangeLinkedChangeNotifierProperties);

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
            OnPropertyChanged(propertyName);

            NotifyLinkedPropertyListeners(propertyName, AfterChangeLinkedChangeNotifierProperties);
        }

        private void NotifyLinkedPropertyListeners(string propertyName,
                                                   Dictionary<string, LinkedPropertyChangeNotifierListeners>
                                                       linkedChangeNotifiers)
        {
            LinkedPropertyChangeNotifierListeners changeNotifierListeners;
            if (linkedChangeNotifiers.TryGetValue(propertyName, out changeNotifierListeners))
            {
                changeNotifierListeners.NotifyAll();
            }
        }

        private void RegisterAfterLinkedPropertyListener(string linkedPropertyName,
                                                         ChangeNotifier targetObject,
                                                         string targetPropertyName)
        {
            RegisterLinkedPropertyListener(linkedPropertyName, targetObject, targetPropertyName,
                                           AfterChangeLinkedChangeNotifierProperties);
        }

        private void RegisterAfterLinkedPropertyListener(string linkedPropertyName,
                                                         ChangeNotifier targetObject,
                                                         Action action)
        {
            RegisterLinkedPropertyListener(linkedPropertyName, targetObject, action,
                                           AfterChangeLinkedChangeNotifierProperties);
        }

        private void RegisterAfterLinkedPropertyListener(string sourcePropertyName,
                                                         INotifyPropertyChanged sourceNotifyObject,
                                                         string targetPropertyName)
        {
            GetOrCreateLinkedPropertyStandardNotifierListeners(targetPropertyName)
                .Register(sourceNotifyObject, sourcePropertyName, this, targetPropertyName);
        }

        private void RegisterAfterLinkedPropertyListener(string sourcePropertyName,
                                                         INotifyPropertyChanged sourceNotifyObject,
                                                         Action action)
        {
            GetOrCreateLinkedPropertyStandardNotifierListeners(sourcePropertyName)
                .Register(sourceNotifyObject, sourcePropertyName, this, action);
        }

        private LinkedPropertyStandardNotifierListeners GetOrCreateLinkedPropertyStandardNotifierListeners(
            string targetPropertyName)
        {
            LinkedPropertyStandardNotifierListeners linkedProperties;
            if (!AfterChangeLinkedStandardNotifierProperties.TryGetValue(targetPropertyName, out linkedProperties))
            {
                linkedProperties = new LinkedPropertyStandardNotifierListeners();
                AfterChangeLinkedStandardNotifierProperties.Add(targetPropertyName, linkedProperties);
            }
            return linkedProperties;
        }

        private void RegisterBeforeLinkedPropertyListener(string linkedPropertyName,
                                                          ChangeNotifier targetObject,
                                                          string targetPropertyName)
        {
            RegisterLinkedPropertyListener(linkedPropertyName, targetObject, targetPropertyName,
                                           BeforeChangeLinkedChangeNotifierProperties);
        }

        private void RegisterBeforeLinkedPropertyListener(string linkedPropertyName,
                                                          ChangeNotifier targetObject,
                                                          Action action)
        {
            RegisterLinkedPropertyListener(linkedPropertyName, targetObject, action,
                                           BeforeChangeLinkedChangeNotifierProperties);
        }


        private class AfterLinkedPropertyChanged : ILinkedPropertyChanged
        {
            private readonly ChangeNotifier _sourceChangeNotifier;
            private readonly string _sourceProperty;
            private readonly ChangeNotifier _targetChangeNotifier;
            private readonly INotifyPropertyChanged _notifyPropertyChanged;

            public AfterLinkedPropertyChanged(INotifyPropertyChanged sourceChangeNotifier,
                                              string sourceProperty,
                                              ChangeNotifier targetChangeNotifier)
            {
                _sourceChangeNotifier = sourceChangeNotifier as ChangeNotifier;
                _notifyPropertyChanged = sourceChangeNotifier;
                _sourceProperty = sourceProperty;
                _targetChangeNotifier = targetChangeNotifier;
            }

            public ILinkedPropertyChanged Notify(Expression<Func<object>> targetProperty)
            {
                var targetPropertyName = targetProperty.GetName();

                if (_sourceChangeNotifier != null)
                {
                    _sourceChangeNotifier.RegisterAfterLinkedPropertyListener(_sourceProperty, _targetChangeNotifier,
                                                                              (string) targetPropertyName);
                }
                else
                {
                    _targetChangeNotifier.RegisterAfterLinkedPropertyListener(_sourceProperty, _notifyPropertyChanged,
                                                                              (string) targetPropertyName);
                }
                return this;
            }

            public ILinkedPropertyChanged Execute(Action action)
            {
                if (_sourceChangeNotifier != null)
                {
                    _sourceChangeNotifier.RegisterAfterLinkedPropertyListener(_sourceProperty, _targetChangeNotifier,
                                                                              action);
                }
                else
                {
                    _targetChangeNotifier.RegisterAfterLinkedPropertyListener(_sourceProperty, _notifyPropertyChanged,
                                                                              action);
                }
                return this;
            }
        }

        private class BeforeLinkedPropertyChanged : ILinkedPropertyChanged
        {
            private readonly ChangeNotifier _sourceChangeNotifier;
            private readonly string _sourceProperty;
            private readonly ChangeNotifier _targetChangeNotifier;

            public BeforeLinkedPropertyChanged(ChangeNotifier sourceChangeNotifier,
                                               string sourceProperty,
                                               ChangeNotifier targetChangeNotifier)
            {
                _sourceChangeNotifier = sourceChangeNotifier;
                _sourceProperty = sourceProperty;
                _targetChangeNotifier = targetChangeNotifier;
            }

            public ILinkedPropertyChanged Notify(Expression<Func<object>> targetProperty)
            {
                _sourceChangeNotifier.RegisterBeforeLinkedPropertyListener(_sourceProperty, _targetChangeNotifier,
                                                                           (string) targetProperty.GetName());
                return this;
            }

            public ILinkedPropertyChanged Execute(Action action)
            {
                _sourceChangeNotifier.RegisterBeforeLinkedPropertyListener(_sourceProperty, _targetChangeNotifier,
                                                                           action);
                return this;
            }
        }

        private class LinkedObjectChanged : ILinkedObjectChanged
        {
            private readonly ChangeNotifier _targetChangeNotifier;
            private readonly string _targetPropertyName;

            public LinkedObjectChanged(ChangeNotifier targetChangeNotifier, string targetPropertyName)
            {
                _targetChangeNotifier = targetChangeNotifier;
                _targetPropertyName = targetPropertyName;
            }

            public ILinkedObjectChanged AfterNotify(Expression<Func<object>> sourceProperty)
            {
                _targetChangeNotifier.RegisterAfterLinkedPropertyListener(sourceProperty.GetName(),
                                                                          _targetChangeNotifier, _targetPropertyName);
                return this;
            }

            public ILinkedObjectChanged AfterNotify<T>(T sourceChangeNotifier,
                                                       Expression<Func<T, object>> sourceProperty)
                where T : INotifyPropertyChanged
            {
                var sourcePropertyName = sourceProperty.GetName();

                if (sourceChangeNotifier is ChangeNotifier)
                {
                    (sourceChangeNotifier as ChangeNotifier).RegisterAfterLinkedPropertyListener(sourcePropertyName,
                                                                                                 _targetChangeNotifier,
                                                                                                 _targetPropertyName);
                }
                else
                {
                    _targetChangeNotifier.RegisterAfterLinkedPropertyListener(sourcePropertyName, sourceChangeNotifier,
                                                                              _targetPropertyName);
                }
                return this;
            }

            public ILinkedObjectChanged BeforeNotify<T>(T sourceChangeNotifier,
                                                        Expression<Func<T, object>> sourceProperty)
                where T : ChangeNotifier
            {
                sourceChangeNotifier.RegisterBeforeLinkedPropertyListener(sourceProperty.GetName(),
                                                                          _targetChangeNotifier, _targetPropertyName);
                return this;
            }

            public ILinkedObjectChanged BeforeNotify(Expression<Func<object>> sourceProperty)
            {
                _targetChangeNotifier.RegisterBeforeLinkedPropertyListener(sourceProperty.GetName(),
                                                                           _targetChangeNotifier, _targetPropertyName);
                return this;
            }
        }

        private class LinkedPropertyChangeNotifierListeners
        {
            private readonly Dictionary<ChangeNotifier, OnNotifyExecuties> _linkedObjects =
                new Dictionary<ChangeNotifier, OnNotifyExecuties>();

            public void Register(ChangeNotifier linkedObject, string targetPropertyName)
            {
                var executies = GetOrCreateExecuties(linkedObject);

                if (!executies.ProprtiesToNotify.Contains(targetPropertyName))
                {
                    executies.ProprtiesToNotify.Add(targetPropertyName);
                }
            }

            public void Register(ChangeNotifier linkedObject, Action action)
            {
                var executies = GetOrCreateExecuties(linkedObject);

                if (!executies.ActionsToExecute.Contains(action))
                {
                    executies.ActionsToExecute.Add(action);
                }
            }

            private OnNotifyExecuties GetOrCreateExecuties(ChangeNotifier linkedObject)
            {
                OnNotifyExecuties executies;
                if (!_linkedObjects.TryGetValue(linkedObject, out executies))
                {
                    executies = new OnNotifyExecuties();
                    _linkedObjects.Add(linkedObject, executies);
                }
                return executies;
            }

            public void NotifyAll()
            {
                foreach (var linkedObject in _linkedObjects)
                {
                    NotifyProperties(linkedObject.Key, linkedObject.Value.ProprtiesToNotify);
                    ExecuteActions(linkedObject.Value.ActionsToExecute);
                }
            }

            private void NotifyProperties(ChangeNotifier linkedObject, IEnumerable<string> properties)
            {
                foreach (var targetProperty in properties)
                {
                    linkedObject.NotifyPropertyChangedInternal(targetProperty);
                }
            }

            private void ExecuteActions(IEnumerable<Action> actions)
            {
                foreach (var action in actions)
                {
                    action();
                }
            }

            private class OnNotifyExecuties
            {
                private List<string> _proprtiesToNotify;
                private List<Action> _actionsToExecute;

                public List<string> ProprtiesToNotify
                {
                    get { return _proprtiesToNotify ?? (_proprtiesToNotify = new List<string>()); }
                }

                public List<Action> ActionsToExecute
                {
                    get { return _actionsToExecute ?? (_actionsToExecute = new List<Action>()); }
                }
            }
        }

        private class LinkedPropertyStandardNotifierListeners
        {
            private readonly Dictionary<INotifyPropertyChanged, StandardNotifierListeners> _linkedObjects =
                new Dictionary<INotifyPropertyChanged, StandardNotifierListeners>();

            public void Register(INotifyPropertyChanged sourceNotifyObject,
                                 string sourcePropertyName,
                                 ChangeNotifier changeNotifier,
                                 string targetPropertyName)
            {
                GetOrCreateStandardNotifierListeners(sourceNotifyObject).Register(sourcePropertyName, changeNotifier, targetPropertyName);
            }

            public void Register(INotifyPropertyChanged sourceNotifyObject,
                                 string sourcePropertyName,
                                 ChangeNotifier changeNotifier,
                                 Action action)
            {
                GetOrCreateStandardNotifierListeners(sourceNotifyObject).Register(sourcePropertyName, changeNotifier, action);
            }

            private StandardNotifierListeners GetOrCreateStandardNotifierListeners(INotifyPropertyChanged sourceNotifyObject)
            {
                StandardNotifierListeners standardNotifierListeners;
                if (!_linkedObjects.TryGetValue(sourceNotifyObject, out standardNotifierListeners))
                {
                    sourceNotifyObject.PropertyChanged += OnSourceNotifyObjectPropertyChanged;
                    standardNotifierListeners = new StandardNotifierListeners();
                    _linkedObjects.Add(sourceNotifyObject, standardNotifierListeners);
                }
                return standardNotifierListeners;
            }

            private void OnSourceNotifyObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                var sourceNotifyObject = (INotifyPropertyChanged) sender;
                _linkedObjects[sourceNotifyObject].NotifyAll(e.PropertyName);
            }

            private class StandardNotifierListeners
            {
                private readonly Dictionary<string, LinkedPropertyChangeNotifierListeners> _linkedListeners =
                    new Dictionary<string, LinkedPropertyChangeNotifierListeners>();

                public void Register(string sourcePropertyName, ChangeNotifier changeNotifier, string targetPropertyName)
                {
                    GetOrCreateLinkedPropertyChangeNotifierListeners(sourcePropertyName).Register(changeNotifier, targetPropertyName);
                }

                public void Register(string sourcePropertyName, ChangeNotifier changeNotifier, Action action)
                {
                    GetOrCreateLinkedPropertyChangeNotifierListeners(sourcePropertyName).Register(changeNotifier, action);
                }

                private LinkedPropertyChangeNotifierListeners GetOrCreateLinkedPropertyChangeNotifierListeners(string sourcePropertyName)
                {
                    LinkedPropertyChangeNotifierListeners linkedChangeNotifiers;
                    if (!_linkedListeners.TryGetValue(sourcePropertyName, out linkedChangeNotifiers))
                    {
                        linkedChangeNotifiers = new LinkedPropertyChangeNotifierListeners();
                        _linkedListeners.Add(sourcePropertyName, linkedChangeNotifiers);
                    }
                    return linkedChangeNotifiers;
                }

                public void NotifyAll(string propertyName)
                {
                    LinkedPropertyChangeNotifierListeners linkedChangeNotifiers;
                    if (_linkedListeners.TryGetValue(propertyName, out linkedChangeNotifiers))
                    {
                        linkedChangeNotifiers.NotifyAll();
                    }
                }
            }
        }
    }
}
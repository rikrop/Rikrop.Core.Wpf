using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Rikrop.Core.Wpf.MessageRouting
{
    public class MessageListener<TMessage> : IMessageListener<TMessage>, IMessageSender<TMessage>
    {
        private readonly List<EventEntry> _eventEntries;

        public MessageListener()
        {
            _eventEntries = new List<EventEntry>();
        }

        public void Listen(Action<TMessage> listener)
        {
            lock (_eventEntries)
            {
                var d = (Delegate)listener;

                if (d.Method.DeclaringType.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length != 0)
                {
                    throw new ArgumentException("Cannot create weak event to anonymous method with closure.");
                }

                if (_eventEntries.Count == _eventEntries.Capacity)
                {
                    RemoveDeadEntries();
                }
                var target = d.Target != null
                                 ? new WeakReference(d.Target)
                                 : null;
                _eventEntries.Add(new EventEntry(d.Method, target));
            }
        }

        public void StopListen(Action<TMessage> listener)
        {
            lock (_eventEntries)
            {
                var d = (Delegate)listener;
                for (var i = _eventEntries.Count - 1; i >= 0; i--)
                {
                    var entry = _eventEntries[i];
                    if (entry.TargetReference != null)
                    {
                        var target = entry.TargetReference.Target;
                        if (target == null)
                        {
                            _eventEntries.RemoveAt(i);
                        }
                        else if (target == d.Target && entry.TargetMethod == d.Method)
                        {
                            _eventEntries.RemoveAt(i);
                            break;
                        }
                    }
                    else
                    {
                        if (d.Target == null && entry.TargetMethod == d.Method)
                        {
                            _eventEntries.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        public void SendMessage(TMessage message)
        {
            var needsCleanup = false;
            object[] parameters = {message};
            IEnumerable<EventEntry> eventEntries;

            lock (_eventEntries)
            {
                eventEntries = _eventEntries.ToArray();
            }

            foreach (var ee in eventEntries)
            {
                if (ee.TargetReference != null)
                {
                    var target = ee.TargetReference.Target;
                    if (target != null)
                    {
                        ee.TargetMethod.Invoke(target, parameters);
                    }
                    else
                    {
                        needsCleanup = true;
                    }
                }
                else
                {
                    ee.TargetMethod.Invoke(null, parameters);
                }
            }
            if (needsCleanup)
            {
                lock (eventEntries)
                {
                    RemoveDeadEntries();
                }
            }
        }

        private void RemoveDeadEntries()
        {
            _eventEntries.RemoveAll(ee => ee.TargetReference != null && !ee.TargetReference.IsAlive);
        }

        private struct EventEntry
        {
            private readonly MethodInfo _targetMethod;
            private readonly WeakReference _targetReference;

            public MethodInfo TargetMethod
            {
                get { return _targetMethod; }
            }

            public WeakReference TargetReference
            {
                get { return _targetReference; }
            }

            public EventEntry(MethodInfo targetMethod, WeakReference targetReference)
            {
                _targetMethod = targetMethod;
                _targetReference = targetReference;
            }
        }
    }
}
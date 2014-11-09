using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace Rikrop.Core.Wpf.Helpers.WeekEvents
{
    public class Listener<TEventArgs> : IWeakEventListener where TEventArgs : EventArgs
    {
        private readonly EventHandler<TEventArgs> _realHandler;

        public Listener(EventHandler<TEventArgs> handler)
        {
            Contract.Requires<ArgumentNullException>(handler != null);
            _realHandler = handler;
        }

        #region IWeakEventListener Members

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, Object sender, EventArgs e)
        {
            var realArgs = (TEventArgs) e;
            _realHandler(sender, realArgs);
            return true;
        }

        #endregion
    }
}
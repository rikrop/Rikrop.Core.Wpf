using System.Windows;

namespace Rikrop.Core.Wpf
{
    public abstract class WeakEventManagerBase<TManager, TEventSource> : WeakEventManager
        where TManager : WeakEventManagerBase<TManager, TEventSource>, new()
        where TEventSource : class
    {
        /// <summary>
        /// Adds a listener
        /// </summary>
        /// <param name="source">The source of the event, should be null if listening to static events</param>
        /// <param name="listener">The listener of the event. This is the class that will recieve the ReceiveWeakEvent method call</param>
        public static void AddListener(TEventSource source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(source, listener);
        }

        /// <summary>
        /// Removes a listener
        /// </summary>
        /// <param name="source">The source of the event, should be null if listening to static events</param>
        /// <param name="listener">The listener of the event. This is the class that will recieve the ReceiveWeakEvent method call</param>
        public static void RemoveListener(TEventSource source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        /// <inheritdoc/>
        protected sealed override void StartListening(object source)
        {
            StartListeningTo((TEventSource)source);
        }

        /// <inheritdoc/>
        protected sealed override void StopListening(object source)
        {
            StopListeningTo((TEventSource)source);
        }

        /// <summary>
        /// Attaches the event handler.
        /// </summary>
        protected abstract void StartListeningTo(TEventSource source);

        /// <summary>
        /// Detaches the event handler.
        /// </summary>
        protected abstract void StopListeningTo(TEventSource source);

        /// <summary>
        /// Gets the current manager
        /// </summary>
        protected static TManager CurrentManager
        {
            get
            {
                var mType = typeof(TManager);
                var mgr = (TManager)GetCurrentManager(mType);
                if (mgr == null)
                {
                    mgr = new TManager();
                    SetCurrentManager(mType, mgr);
                }
                return mgr;
            }
        }
    }
}

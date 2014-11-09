using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Rikrop.Core.Wpf.Async
{
    public class CompositeBusyTrigger : IBusyTrigger
    {
        private readonly IReadOnlyCollection<IBusyTrigger> _triggers;

        public CompositeBusyTrigger(IReadOnlyCollection<IBusyTrigger> triggers)
        {
            Contract.Requires<ArgumentNullException>(triggers != null);
            
            _triggers = triggers;
        }

        public void SetBusy()
        {
            foreach (var trigger in _triggers)
            {
                trigger.SetBusy();
            }
        }

        public void ClearBusy()
        {
            foreach (var trigger in _triggers)
            {
                trigger.SetBusy();
            }
        }
    }
}
using System;
using System.Diagnostics.Contracts;
using Rikrop.Core.Wpf.MessageRouting.Contracts;

namespace Rikrop.Core.Wpf.MessageRouting
{
    [ContractClass(typeof(MessageListenerContract<>))]
    public interface IMessageListener<out TMessage>
    {
        void Listen(Action<TMessage> listener);

        void StopListen(Action<TMessage> listener);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IMessageListener<>))]
        public abstract class MessageListenerContract<TMessage> : IMessageListener<TMessage>
        {
            public void Listen(Action<TMessage> listener)
            {
                Contract.Requires<ArgumentNullException>( listener != null);
            }

            public void StopListen(Action<TMessage> listener)
            {
                Contract.Requires<ArgumentNullException>( listener != null);
            }
        }
    }
}
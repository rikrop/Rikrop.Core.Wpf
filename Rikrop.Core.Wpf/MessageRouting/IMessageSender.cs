using System;
using System.Diagnostics.Contracts;
using Rikrop.Core.Wpf.MessageRouting.Contracts;

namespace Rikrop.Core.Wpf.MessageRouting
{
    [ContractClass(typeof(MessageSenderContract<>))]
    public interface IMessageSender<in TMessage>
    {
        void SendMessage(TMessage message);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IMessageSender<>))]
        public abstract class MessageSenderContract<TMessage> : IMessageSender<TMessage>
        {
            public void SendMessage(TMessage message)
            {
                Contract.Requires<ArgumentNullException>(typeof(TMessage).IsValueType || message != null);
            }
        }
    }
}
using System;
using System.Messaging;

namespace msmq_base_support
{
    /*
     * this is not yet fully thought-out and might be a dead-end...
     * the idea is to standardize the requirements for queueable types, but the
     * specifics are hairy here.
     */
    public interface iQueueableType<T>
    {
        Type[] supportedtypes { get; }

        T parse_msmq_message(Message m);
    }
}

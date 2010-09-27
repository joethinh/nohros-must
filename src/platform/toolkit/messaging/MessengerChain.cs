using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// Represents a chain of messenger to use to send a message using multiples messengers.
    /// </summary>
    /// <remarks></remarks>
    public sealed class MessengerChain
    {
        List<IMessenger> messenger_;

        #region .ctor
        public MessengerChain() {
            messenger_ = new List<IMessenger>();
        }

        static MessengerChain() {
            instance = new MessengerChain();
        }
        #endregion

        /// <summary>
        /// Adds a new messenger to the chain.
        /// </summary>
        /// <param name="messenger">The messenger to add</param>
        public void Add(IMessenger messenger) {
            if (messenger == null)
                throw new ArgumentNullException("messenger");
            messenger_.Add(messenger);
        }
    }
}

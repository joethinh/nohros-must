using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Logging;
using Nohros.Configuration;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// Represents a chain of messenger to use to send a message using multiples messengers.
    /// </summary>
    /// <remarks></remarks>
    public class MessengerChain
    {
        List<IMessenger> messengers_;
        string name_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the MessengerChain, using the specified chain name.
        /// </summary>
        public MessengerChain(string name) {
            name_ name;
            messengers_ = new List<IMessenger>();
        }
        #endregion

        /// <summary>
        /// Creates a MessengerChain class instance by using the specified chain name and configuration object.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>An instance of the MessengerChain class with name <paramref name="name"/> configured
        /// accordingly to the specified configuration object.</returns>
        /// <remarks>
        /// This method will try to get a chain with name <paramref name="name"/> from the configuration file
        /// and loop through the chain trying to get messengers with name equals to the name of each chain node.
        /// <para>
        /// If a chain with name <paramref name="name"/> was not found. The returned chain will have no messegers.
        /// </para>
        /// </remarks>
        MessengerChain FromConfiguration(string name, NohrosConfiguration config) {
            ChainNode chain = config.Chains[name];
            if (chain != null) {
                string[] nodes = chain.Nodes;
                for (int i = 0, j = nodes.Length; i < j; i++) {
                    MessengerProviderNode node = config.MessengerProviders[nodes[i]];
                    if (node != null) {
                        Add(CreateInstance(node));
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new messenger to the chain.
        /// </summary>
        /// <param name="messenger">The messenger to add to the chain.</param>
        public void Add(IMessenger messenger) {
            if (messenger == null)
                throw new ArgumentNullException("messenger");
            messengers_.Add(messenger);
        }

        /// <summary>
        /// Sends a message using the messengers of the chain.
        /// </summary>
        /// <returns>An list containing the errors generated on send operation.</returns>
        /// <remarks>
        /// This method does not check the message validity. It is the responsability of the
        /// messenger to validate the message and, if is the case, does not send it. If a messenger
        /// does not send a message for some reason the send method must return a null reference or
        /// a <see cref="ErrorMessage"/> explaining the error.
        /// </remarks>
        List<ErrorMessage> Send(IMessage message) {
            List<ErrorMessage> errors = new List<ErrorMessage>();

            foreach (IMessenger messenger in messengers_) {
                // the try/catch block is used here to ensure that
                // the message is received by all messengers.
                try {
                    IMessage response = messenger.Send(message);
                    if (response != null)
                        messenger.ProcessResponse(response);
                } catch(Exception exception) {
                    FileLogger.ForCurrentProcess.Logger.Error("[Send   Nohros.Toolkit.Messaging.MessengerChain]", exception);
                    errors.Add(new ErrorMessage(ex.Message));
                }
            }
            return errors;
        }

        /// <summary>
        /// Gets the number of <see cref="IMessenger"/> object actually contained in the MessengerChain.
        /// </summary>
        public int Count {
            get { return messengers_.Count; }
        }
    }
}

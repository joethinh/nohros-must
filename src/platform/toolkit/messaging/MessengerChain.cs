using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Logging;
using Nohros.Configuration;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// Represents a chain of messengers.Its is typically used to send a message using multiples
    /// messengers(broadcast messages).
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
            name_ = name;
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
        /// <para>
        /// If a messenger of a chain could not be instantiate for any reason a <see cref="ProviderException"/> is
        /// throw.
        /// </para>
        /// </remarks>
        public static MessengerChain FromConfiguration(string name, NohrosConfiguration config) {
            MessengerChain messenger_chain = new MessengerChain(name);
            ChainNode chain = config.Chains[name];
            if (chain != null) {
                string[] nodes = chain.Nodes;
                for (int i = 0, j = nodes.Length; i < j; i++) {
                    MessengerProviderNode node = config.MessengerProviders[nodes[i]];
                    if (node != null)
                        messenger_chain.Add(Messenger.CreateInstance(node));
                }
            }
            return messenger_chain;
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
        List<ResponseMessage> Send(IMessage message) {
            List<ResponseMessage> errors = new List<ResponseMessage>();

            foreach (IMessenger messenger in messengers_) {
                // the try/catch block is used here to ensure that the message
                // is delivered to all messengers.
                try {
                    IMessage response = messenger.Send(message);
                    OnProcessResponse(messenger, response);
                } catch(Exception exception) {
                    errors.Add(new ResponseMessage(exception.Message, ResponseMessageType.ErrorMessage));
                }
            }
            return errors;
        }

        void OnProcessResponse(IMessenger messenger, IMessage response) {
            if (ProcessResponse != null && response != null) {
                ProcessResponse(messenger, response);
            }
        }

        /// <summary>
        /// Occurs when a messenger sents a message.
        /// </summary>
        public event ProcessResponseEventHandler ProcessResponse;

        /// <summary>
        /// Gets the number of <see cref="IMessenger"/> objects actually contained in the MessengerChain.
        /// </summary>
        public int Count {
            get { return messengers_.Count; }
        }
    }
}

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
        static MessengerChain current_process_chain_;

        List<IMessenger> messengers_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the MessengerChain.
        /// </summary>
        public MessengerChain() {
            messengers_ = new List<IMessenger>();
        }

        /// <summary>
        /// Initializes the sngleton process instance of the MessengerChain class.
        /// </summary>
        static MessengerChain() {
            current_process_chain_ = new MessengerChain();
        }
        #endregion

        /// <summary>
        /// Adds a new messenger to the chain.
        /// </summary>
        /// <param name="messenger">The messenger to add</param>
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
                } catch(Exception ex) {
                    // TODO: log the exception.
                    errors.Add(new ErrorMessage(ex.Message));
                }
            }
            return errors;
        }

        /// <summary>
        /// Gets the current process's messenger chain.
        /// </summary>
        static MessengerChain ForCurrentProcess
        {
            get { return current_process_chain_; }
        }
    }
}

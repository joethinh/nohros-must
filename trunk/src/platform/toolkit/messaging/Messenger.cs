using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Configuration;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// Serves as the base class for custom <see cref="Nohros.Toolkit.IMessenger"/>.
    /// </summary>
    public class Messenger : IMessenger
    {
        #region .ctor
        /// <summary>
        /// Initializes a new instance of the Messenger class.
        /// </summary>
        public Messenger() { }
        #endregion

        /// <summary>
        /// Creates an instance of the type designated by the specified generic type parameter using the
        /// constructor implied by the <see cref="IMessenger"/> interface.
        /// </summary>
        /// <param name="provider">A <see cref="MessengerProvider"/> object containing informations
        /// that will be used to create the designated type.</param>
        /// <returns>A reference to the newly created object.</returns>
        /// <remarks>
        /// The <typeparamref name="T"/> parameter must be a class that implements or derive from a class
        /// that implements the <see cref="IMessenger"/> interface.
        /// <para>
        /// The type T must have a parameterless constructor.
        /// </para>
        /// <para>If the <see cref="Provider.Location"/> of the specified provider is null this method will try
        /// to load the assembly associated with the provider type from the application base directory.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> is null</exception>
        /// <exception cref="ProviderException">The type could not be created.</exception>
        /// <exception cref="ProviderException"><paramref name="provider"/> is invalid.</exception>
        public IMessenger CreateInstace(MessengerProviderNode provider) {
            if (provider == null)
                throw new ArgumentNullException("provider");

            IMessenger new_object;
            Type type = ProviderHelper.GetTypeFromProviderNode(provider);
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <returns>A <see cref="IMessage"/> containing the response from the messaging system.</returns>
        public abstract IMessage Send(IMessage message);

        /// <summary>
        /// Process the message response sent from the messaging system.
        /// </summary>
        /// <param name="message">The response message to process</param>
        /// <remarks>This method is used to process response message that could be sent
        /// from messaging system after a message is sent, when a applications needs to
        /// performs some post processing operation(ex. store the response into a database).</remarks>
        public abstract void ProcessResponse(IMessage message);
    }
}

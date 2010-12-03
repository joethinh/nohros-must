using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Data.Providers;
using Nohros.Configuration;
using Nohros.Resources;

namespace Nohros.Toolkit.Messaging
{
    /// <summary>
    /// Serves as the base class for custom <see cref="IMessenger"/>.
    /// </summary>
    public sealed class Messenger
    {
        /// <summary>
        /// Creates an instance of the type designated by the specified generic type parameter using the
        /// constructor implied by the <see cref="IMessenger"/> interface.
        /// </summary>
        /// <param name="provider">A <see cref="MessengerProviderNode"/> object containing informations
        /// that will be used to create the designated type.</param>
        /// <returns>A reference to the newly created object.</returns>
        /// <remarks>
        /// The <typeparamref name="T"/> parameter must be a class that implements or derive from a class
        /// that implements the <see cref="IMessenger"/> interface.
        /// <para>
        /// The type T must have a constructor that accepts a string and a <see cref="IDictionary&lt;TKey, TValue&gt;"/>
        /// object, where TKey and TValue are both strings. The string parameter represents the name of the provider and
        /// the dictionary parameter represents the options configured for the provider. Note that the options parameter
        /// could be a null reference, but the name parameter could not. If the name parameter is null the constructor
        /// should throw an <see cref="ArgumentNullException"/> exception.</para>
        /// <para>
        /// If the constructor of the type beign instantiated throws an exception it won't be propagated to the caller.
        /// The CreateInstance method will pack the exception into a <see cref="ProviderException"/> and rethrow it.
        /// exception and throw it.
        /// </para>
        /// <para>If the Location property of the specified provider is null this method will try
        /// to load the assembly associated with the provider type from the application base directory.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> is null</exception>
        /// <exception cref="ProviderException">The type could not be created.</exception>
        /// <exception cref="ProviderException"><paramref name="provider"/> is invalid.</exception>
        public static IMessenger CreateInstance(MessengerProviderNode provider) {
            if (provider == null)
                throw new ArgumentNullException("provider");

            Exception exception = null;

            IMessenger new_object = null;
            Type type = ProviderHelper.GetTypeFromProviderNode(provider);
            if (type != null)
                try {
                    new_object = Activator.CreateInstance(type, provider.Name, provider.Options) as IMessenger;
                } catch (Exception e) { exception = e; }

            if (new_object == null || exception != null)
                throw new ProviderException(string.Format(StringResources.Type_CreateInstanceOf, "IMessenger"), exception);

            return new_object;
        }

        /// <summary>
        /// Searchs for an option with name <paramref name="option"/> into the the specified options collection and
        /// retrieve it's value.
        /// </summary>
        /// <param name="option">The name of the option to get.</param>
        /// <param name="options">A <see cref="IDictionary&lt;string,string&gt;"/> that represents the
        /// options collection.</param>
        /// <returns>An string that is the value of the option with name <paramref name="option"/>.</returns>
        /// <remarks>
        /// If an option with name <paramref name="option"/> is not found within the <paramref name="options"/>
        /// collection, this method throws an <see cref="ArgumentException"/> exception.
        /// </remarks>
        /// <exception cref="ArgumentException">An option with name <paramref name="option"/> is not found
        /// within the <paramref name="options"/> collection.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="options"/> is null.</exception>
        public static string GetRequiredOption(string option, IDictionary<string, string> options) {
            if (options == null)
                throw new ArgumentNullException("options");

            string value;
            if (!options.TryGetValue(option, out value))
                throw new ArgumentException(string.Format(StringResources.Provider_Option_MissingAt, option));
            return value;
        }
    }
}

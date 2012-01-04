using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Data.Providers;
using Nohros.Configuration;
using Nohros.Resources;

namespace Nohros.Toolkit.Messaging
{
  /// <summary>
  /// A set of usefull methods for implementors of the <see cref="IMessenger"/>
  /// interface.
  /// </summary>
  public sealed class Messenger
  {
    /// <summary>
    /// Creates an instance of the <see cref="IMessenger"/> class by using the
    /// factory class that was defined in the specified configuration node.
    /// </summary>
    /// <param name="provider">A <see cref="MessengerProviderNode"/> object
    /// containing informations that will be used to create the designated
    /// type.</param>
    /// <returns>A reference to the newly created object.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="provider"/>
    /// is null.</exception>
    /// <exception cref="ProviderException">A valid instance of the factory
    /// could not be created -or - a exception was throwed by the factory.
    /// </exception>
    /// <remarks>
    /// <para>
    /// If the factory could not be instantiated or if it throws an exception
    /// it won't be propagated to the caller. This method packs the exception
    /// into a <see cref="ProviderException"/> and throw it.
    /// </para>
    /// </remarks>
    public static IMessenger CreateInstance(MessengerProviderNode provider) {
      if (provider == null)
        throw new ArgumentNullException("provider");

      IMessengerFactory factory;
      Type type = ProviderHelper.GetTypeFromProviderNode(provider);
      if (type == null) {
        throw new ProviderException(string.Format(
          StringResources.Type_CreateInstanceOf, "IMessengerFactory"));
      }
      
      try {
        factory = Activator.CreateInstance(type) as IMessengerFactory;
        if (factory == null) {
          throw new ProviderException(string.Format(
            StringResources.Type_CreateInstanceOf, "IMessengerFactory"));
        }

        IMessenger messenger =
          factory.CreateMessenger(provider.Name, provider.Options);

        // Sanitu check for nulls. By default messenger factories should not
        // return nulls, but we can trust on others code.
        if (messenger == null) {
            throw new ProviderException(string.Format(
              StringResources.Type_CreateInstanceOf, "IMessengerFactory"));
        }
        return messenger;
      } catch (Exception e) {
        // pack the throwed exception and rethrow it.
        throw new ProviderException(string.Format(
          StringResources.Type_CreateInstanceOf, "IMessenger"), e);
      }
    }

    /// <summary>
    /// Searchs for an option with name <paramref name="option"/> into the the
    /// specified options collection and retrieve it's value.
    /// </summary>
    /// <param name="option">The name of the option to get.</param>
    /// <param name="options">A <see cref="IDictionary&lt;string,string&gt;"/>
    /// that represents the options collection.</param>
    /// <returns>An string that is the value of the option with name
    /// <paramref name="option"/>.</returns>
    /// <exception cref="ArgumentException">An option with name
    /// <paramref name="option"/> is not found within the
    /// <paramref name="options"/> collection.</exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="options"/> is null.</exception>
    /// <remarks>
    /// Required options are options that must exists to a messenger work
    /// properly. If an option with name <paramref name="option"/> is not
    /// found within the <paramref name="options"/> collection, this method
    /// throws an <see cref="ArgumentException"/> exception.
    /// </remarks>
    public static string GetRequiredOption(string option,
      IDictionary<string, string> options) {
      if (options == null)
        throw new ArgumentNullException("options");

      string value;
      if (!options.TryGetValue(option, out value))
        throw new ArgumentException(
          string.Format(StringResources.Provider_Option_MissingAt, option));
      return value;
    }
  }
}

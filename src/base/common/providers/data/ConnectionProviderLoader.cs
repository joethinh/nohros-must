using System;
using System.Collections.Generic;

using Nohros.Caching;
using Nohros.Configuration;
using Nohros.Data.Providers;
using Nohros.Providers;
using Nohros.Resources;

namespace Nohros.Data
{
  /// <summary>
  /// A <see cref="CacheLoader{T}"/> that can be used to dynamically load
  /// instances of the <see cref="IConnectionProvider"/> class using the
  /// provider name.
  /// </summary>
  public class ConnectionProviderLoader : CacheLoader<IConnectionProvider>
  {
    readonly Dictionary<string, IProviderNode> providers_;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionProviderLoader"/>
    /// class that contains the elements copied from the specified collection.
    /// </summary>
    /// <param name="providers">
    /// The collection whose elements are copied to the new
    /// <see cref="ConnectionProviderLoader"/> object.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="providers"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="providers"/> contains one or more null elements.
    /// </exception>
    public ConnectionProviderLoader(IEnumerable<IProviderNode> providers) {
      if (providers == null) {
        throw new ArgumentNullException("providers");
      }
      providers_ = new Dictionary<string, IProviderNode>();
      foreach(IProviderNode provider in providers) {
        if (provider == null) {
          throw new ArgumentException(
            string.Format(StringResources.Argument_CollectionNoNulls,
              "providers"), "providers");
        }
        providers_.Add(provider.Name, provider);
      }
    }

    /// <summary>
    /// Creates an instance of the <see cref="IConnectionProvider"/> class.
    /// <paramref name="key"/>.
    /// </summary>
    /// <param name="key">
    /// The name of the provider that should be created
    /// </param>
    /// <returns>
    /// The newly created <see cref="IConnectionProvider"/> object.
    /// </returns>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <remarks>
    /// This method used the <paramref name="key"/> as a index into the list
    /// of providers that was specified in constructor and them uses the found
    /// <see cref=" IProviderNode"/> to construct an instance of the
    /// <see cref="IConnectionProvider"/> class, if a provider with name
    /// <paramref name="key"/> is not found, this method raises a
    /// <see cref="KeyNotFoundException"/> exception.
    /// </remarks>
    public override IConnectionProvider Load(string key) {
      IProviderNode provider = providers_[key];
      return ProviderFactory<IConnectionProviderFactory>
        .CreateProviderFactory(provider)
        .CreateProvider(provider.Options);
    }
  }
}

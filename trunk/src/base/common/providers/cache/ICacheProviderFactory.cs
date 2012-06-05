using System;
using System.Collections.Generic;

namespace Nohros.Caching.Providers
{
  /// <summary>
  /// Defines the methods and properties that is used to create instances of
  /// the <see cref="ICacheProvider"/> class.
  /// </summary>
  /// <remarks>
  /// This interface implies a constructor with no parameters.
  /// </remarks>
  public interface ICacheProviderFactory
  {
    /// <summary>
    /// Creates an instance of the <see cref="ICacheProvider"/> using the
    /// specified provider options.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> containing a collection
    /// options configured for the provider.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="ICacheProvider"/> class.
    /// </returns>
    ICacheProvider CreateCacheProvider(IDictionary<string, string> options);
  }
}

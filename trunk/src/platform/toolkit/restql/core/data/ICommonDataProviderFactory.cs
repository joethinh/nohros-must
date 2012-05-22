using System;
using System.Collections.Generic;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// A factory used to create instances of the
  /// <see cref="ICommonDataProvider"/> class.
  /// </summary>
  public interface ICommonDataProviderFactory
  {
    /// <summary>
    /// Creates an instance of the <see cref="ICommonDataProvider"/> object
    /// using the specified provider options and applicaation settings.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> containing the provider's
    /// configured options.
    /// </param>
    /// <param name="settings">
    /// A <see cref="ISettings"/> object containing the application settings.
    /// </param>
    /// <returns></returns>
    ICommonDataProvider CreateCommonDataProvider(
      IDictionary<string, string> options, ISettings settings);
  }
}

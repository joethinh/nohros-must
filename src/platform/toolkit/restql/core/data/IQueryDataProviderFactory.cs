using System;
using System.Collections.Generic;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// A factory used to create instances of the
  /// <see cref="IQueryDataProvider"/> class.
  /// </summary>
  public interface IQueryDataProviderFactory
  {
    /// <summary>
    /// Creates an instance of the <see cref="IQueryDataProvider"/> object
    /// using the specified provider options.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> containing the provider's
    /// configured options.
    /// </param>
    /// <returns></returns>
    IQueryDataProvider CreateCommonDataProvider(
      IDictionary<string, string> options);
  }
}

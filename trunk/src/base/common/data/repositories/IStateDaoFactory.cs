using System;
using System.Collections.Generic;

namespace Nohros.Data
{
  /// <summary>
  /// A factory for the <see cref="IStateDao"/> class.
  /// </summary>
  public interface IStateDaoFactory
  {
    /// <summary>
    /// Creates an instance of the <see cref="IStateDao"/> class by using the
    /// given collection of configured options.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> containing a collection of
    /// key/value pairs representing the provider configuration options
    /// </param>
    /// <returns>
    /// The newly created <see cref="IStateDao"/> object.
    /// </returns>
    IStateDao CreateStateDao(IDictionary<string, string> options);
  }
}

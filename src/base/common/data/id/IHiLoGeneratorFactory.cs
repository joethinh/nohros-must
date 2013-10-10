using System;
using System.Collections.Generic;

namespace Nohros.Data
{
  /// <summary>
  /// Defines a factory for the <see cref="IHiLoGenerator"/> class.
  /// </summary>
  public interface IHiLoGeneratorFactory
  {
    /// <summary>
    /// Creates a new instance of the <see cref="IHiLoGenerator"/> class using
    /// the given user defined configuration options.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> containing the user defined
    /// configuration options.
    /// </param>
    /// <returns>
    /// The newly crated <see cref="IHiLoGenerator"/>.
    /// </returns>
    IHiLoGenerator CreateHiLoGenerator(IDictionary<string, string> options);
  }
}

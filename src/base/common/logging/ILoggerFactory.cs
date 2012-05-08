using System;
using System.Collections.Generic;
using Nohros.Configuration;

namespace Nohros.Logging
{
  /// <summary>
  /// A factory used to create instances of the <see cref="ILogger"/> class.
  /// </summary>
  public interface ILoggerFactory
  {
    /// <summary>
    /// Creates an instance of the <see cref="ILogger"/> class using the
    /// specified provider node.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> object that contains the
    /// options for the logger to be created.
    /// </param>
    /// <param name="configuration">
    /// A <see cref="IMustConfiguration"/> object taht can be used to get
    /// configuration inforamtions related with the logger to be created - such
    /// as the configuration of a related logger.
    /// </param>
    /// <returns>
    /// The newly created <see cref="ILogger"/> object.
    /// </returns>
    ILogger CreateLogger(IDictionary<string, string> options,
      IMustConfiguration configuration);
  }
}

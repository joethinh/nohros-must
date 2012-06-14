using System;
using System.Collections.Generic;
using Nohros.Configuration;

namespace Nohros.Logging.log4net
{
  public partial class ConsoleLogger : ILoggerFactory
  {
    #region .ctor
    /// <summary>
    /// Constructor required by the <see cref="ILoggerFactory"/> interface.
    /// </summary>
    protected ConsoleLogger() {
    }
    #endregion

    #region ILoggerFactory Members
    /// <summary>
    /// Creates an instance of the <see cref="ConsoleLogger"/> using
    /// the specified <paramref name="options"/> object.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> object that contains the
    /// options for the logger to be created.
    /// </param>
    /// <returns>
    /// The newly created <see cref="ILogger"/> object.
    /// </returns>
    public ILogger CreateLogger(IDictionary<string, string> options,
      IMustConfiguration configuration) {
      string layout_pattern = ProviderOptions.GetIfExists(options,
        Strings.kLayoutPattern, kDefaultLogMessagePattern);
      ConsoleLogger logger = new ConsoleLogger(layout_pattern);
      logger.Configure();
      return logger;
    }
    #endregion
  }
}

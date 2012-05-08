using System;
using System.Collections.Generic;
using System.Text;
using Nohros.Configuration;

namespace Nohros.Logging.log4net
{
  public partial class FileLogger : ILoggerFactory
  {
    #region ILoggerFactory Members
    /// <summary>
    /// Creates an instance of the <see cref="FileLogger"/> class using the
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
    /// The newly created <see cref="FileLogger"/> object.
    /// </returns>
    public ILogger CreateLogger(IDictionary<string, string> options,
      IMustConfiguration configuration) {
      string layout_pattern = ProviderOptions.GetIfExists(options,
        Strings.kLayoutPattern, kDefaultLogMessagePattern);
      string log_file_name = ProviderOptions.GetIfExists(options,
        Strings.kLogFileName, kDefaultLogFileName);
      return new FileLogger(layout_pattern, log_file_name);
    }
    #endregion
  }
}

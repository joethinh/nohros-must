using System;
using System.Collections.Generic;
using Nohros.Configuration;

namespace Nohros.Logging.log4net
{
  public class FileLoggerFactory
  {
    /// <summary>
    /// Creates an instance of the <see cref="FileLogger"/> class using the
    /// specified provider node.
    /// </summary>
    /// <param name="options">
    /// A <see cref="IDictionary{TKey,TValue}"/> object that contains the
    /// options for the logger to be created.
    /// </param>
    /// <returns>
    /// The newly created <see cref="FileLogger"/> object.
    /// </returns>
    public ILogger CreateLogger(IDictionary<string, string> options) {
      string layout_pattern = ProviderOptions.GetIfExists(options,
        Strings.kLayoutPattern, AbstractLogger.kDefaultLogMessagePattern);
      string log_file_name = ProviderOptions.GetIfExists(options,
        Strings.kLogFileName, AbstractLogger.kDefaultLogFileName);
      FileLogger logger = new FileLogger(layout_pattern, log_file_name);
      logger.Configure();
      return logger;
    }
  }
}

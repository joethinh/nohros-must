using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Logging
{
  /// <summary>
  /// A generic logger that logs messages a to a text file and the console.
  /// </summary>
  /// <remarks>
  /// This is a generic logger that loads automatically and configures
  /// itself through the code. The messages are logged to the console window.
  /// <para>
  /// The name of the file is "nohros-logger.log".
  /// </para>
  /// <para>
  /// The pattern used to log message are:
  /// . "[%date %-5level/%thread] %message%newline %exception" for the
  /// non-debug messages.
  /// </para>
  /// <para>
  /// The default threshold level is INFO and could be overloaded on the
  /// nohros configuration file.
  /// </para>
  /// </remarks>
  public sealed class ConsoleFileLogger
  {
    const string kFileName = "nohros-logger.log";

    static ILogger current_process_logger_;

    #region .ctor
    /// <summary>
    /// Initializes the singleton process's logger instance.
    /// </summary>
    static ConsoleFileLogger() {
      Log4NetConsoleFileLogger logger = new Log4NetConsoleFileLogger(kFileName);
      logger.Configure();

      current_process_logger_ = logger as ILogger;
    }
    #endregion

    /// <summary>
    /// Gets the current process logger.
    /// </summary>
    public static ILogger ForCurrentProcess {
      get { return current_process_logger_; }
    }
  }
}

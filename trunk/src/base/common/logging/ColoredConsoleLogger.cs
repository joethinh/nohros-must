using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Nohros.Logging
{
  /// <summary>
  /// A generic logger that logs messages to the system console.
  /// </summary>
  /// <remarks>
  /// <see cref="ColoredConsoleLogger"/> appends log events to the
  /// standard output stream ot the error output stream using the folowing
  /// layout:
  /// <para>
  ///   "[%date %-5level/%thread] %message%newline %exception"
  /// </para>
  /// It also allows the color of a specific type of message to be set.
  /// <para>
  /// The default threshold level is INFO and could be overloaded on the
  /// nohros configuration file.
  /// </para>
  /// </remarks>
  public sealed class ColoredConsoleLogger : Log4NetLogger
  {
    const string kLogMessagePattern =
      "[%-5level %date] %message%newline%exception";

    readonly static ILogger current_process_logger_;

    #region .ctor
    /// <summary>
    /// Initializes the singleton process's logger instance.
    /// </summary>
    static ColoredConsoleLogger() {
      Log4NetColoredConsoleLogger logger =
        new Log4NetColoredConsoleLogger(kLogMessagePattern);

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

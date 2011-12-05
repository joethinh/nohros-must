using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Nohros.Logging
{
  /// <summary>
  /// A generic logger that logs messages to a text file.
  /// </summary>
  /// <remarks>
  /// This is a generic logger that loads automatically and configures itself
  /// through the code. The messages are logged to a file that resides on the
  /// same folder of the caller application base directory.
  /// <para>
  /// The name of the file is "nohros-logger.log".
  /// </para>
  /// <para>
  /// The pattern used to log message is:
  ///     . "[%date %-5level/%thread] %message%newline %exception".
  /// </para>
  /// <para>
  /// The default threshold level is DEBUG and could be overloaded on the
  /// configuration file.
  /// </para>
  /// </remarks>
  public class FileLogger
  {
    const string kFileName = "nohros-logger.log";

    static ILogger current_process_logger_;

    #region .ctor
    /// <summary>
    /// Initializes the singleton process's logger instance.
    /// </summary>
    static FileLogger() {
      Log4NetFileLogger logger = new Log4NetFileLogger(kFileName);
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
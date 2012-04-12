using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using log4net;
using log4net.Core;
using log4net.Config;
using log4net.Appender;
using log4net.Layout;
using log4net.Layout.Pattern;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace Nohros.Logging
{
  /// <summary>
  /// A generic logger that uses the third party log4net logging library.
  /// </summary>
  /// <remarks>
  /// This is a generic logger that loads automatically and configures itself
  /// through the code. The messages are logged to the console window.
  /// <para>
  /// The pattern used to log message are:
  ///   . "[%date %-5level/%thread] %message%newline %exception" for the
  ///   non-debug messages.
  /// </para>
  /// <para>
  /// The default threshold level is INFO and could be overloaded on the
  /// nohros configuration file.
  /// </para>
  /// </remarks>
  public class Log4NetConsoleFileLogger: Log4NetLogger
  {
    const string kLogMessagePattern =
      "[%-5level %date] %message%newline%exception";

    string log_file_path_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="Log4NetConsoleFileLogger"/> class.
    /// </summary>
    public Log4NetConsoleFileLogger(string log_file_path) {
#if DEBUG
      if (log_file_path == null || log_file_path.Length == 0) {
        throw new ArgumentException("log_file_path is null or empty");
      }
#endif
      log_file_path_ = log_file_path;
    }
    #endregion

    /// <summary>
    /// Configures the <see cref="Log4NetConsoleFileLogger"/> logger adding
    /// the appenders to the root repository.
    /// </summary>
    public void Configure() {
      // create a new logger into the repository of the current assembly.
      ILoggerRepository root_repository =
        LogManager.GetRepository(Assembly.GetExecutingAssembly());

      Logger nohros_console_logger =
        root_repository.GetLogger("NohrosConsoleLogger") as Logger;

      // create the layout and appender for on error messages.
      PatternLayout layout = new PatternLayout();
      layout.ConversionPattern = kLogMessagePattern;
      layout.ActivateOptions();

      // create the console appender
      ConsoleAppender console_appender = new ConsoleAppender();
      console_appender.Name = "NohrosCommonConsoleAppender";
      console_appender.Layout = layout;
      console_appender.Target = "Console.Out";
      console_appender.Threshold = Level.All;
      console_appender.ActivateOptions();

      // create the file appender
      FileAppender file_appender = new FileAppender();
      file_appender.Name = "NohrosCommonFileAppender";
      file_appender.File = log_file_path_;
      file_appender.AppendToFile = true;
      file_appender.Layout = layout;
      file_appender.Threshold = Level.All;
      file_appender.ActivateOptions();

      nohros_console_logger.Parent.AddAppender(console_appender);
      nohros_console_logger.Parent.AddAppender(file_appender);

      root_repository.Configured = true;

      logger_ = LogManager.GetLogger("NohrosConsoleLogger");
    }
  }
}

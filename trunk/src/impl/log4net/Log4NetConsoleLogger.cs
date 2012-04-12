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
  public class Log4NetConsoleLogger: Log4NetLogger
  {
    const string kLogMessagePattern =
      "[%-5level %date] %message%newline%exception";

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the ConsoleLogger class.
    /// </summary>
    public Log4NetConsoleLogger() { }
    #endregion

    /// <summary>
    /// Configures the <see cref="FileLogger"/> logger adding the appenders
    /// to the root repository.
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

      // create the appender
      ConsoleAppender appender = new ConsoleAppender();
      appender.Name = "NohrosCommonConsoleAppender";
      appender.Layout = layout;
      appender.Target = "Console.Out";
      appender.Threshold = Level.All;
      appender.ActivateOptions();

      nohros_console_logger.Parent.AddAppender(appender);

      root_repository.Configured = true;

      logger_ = LogManager.GetLogger("NohrosConsoleLogger");
    }
  }
}

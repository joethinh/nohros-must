using System;
using System.Reflection;

using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace Nohros.Logging.log4net
{
  /// <summary>
  /// A generic logger that uses the third party log4net logging library.
  /// </summary>
  /// <remarks>
  /// This is a generic logger that loads automatically and configures itself
  /// through the code. The messages are logged to a file that resides on the
  /// same folder of the caller application base directory.The name of the
  /// file is nohros-logger.log.
  /// <para>
  /// The pattern used to log message are:
  ///     . "[%date %-5level/%thread] %message%newline %exception".
  /// </para>
  /// <para>
  /// The default threshold level is INFO and could be overloaded on the nohros
  /// configuration file.
  /// </para>
  /// </remarks>
  public class FileLogger: AbstractLogger
  {
    readonly string log_file_path_;
    readonly string layout_pattern_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the Logger class by using the specified
    /// string as the path to the log file.
    /// </summary>
    /// <remarks>
    /// The logger is not functional at this point, you need to call the
    /// <see cref="Configure"/> method to in order to make the logger usable.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="layout_pattern"/> or <paramref name="log_file_path"/>
    /// are null.
    /// </exception>
    public FileLogger(string layout_pattern, string log_file_path) {
      if (log_file_path == null || layout_pattern == null) {
        throw new ArgumentNullException(log_file_path == null
          ? "log_file_path"
          : "layout_pattern");
      }

      if(log_file_path.Length == 0) {
        throw new ArgumentException("log_file_path");
      }

      log_file_path_ = log_file_path;
      layout_pattern_ = layout_pattern;
    }
    #endregion

    /// <summary>
    /// Configures the <see cref="FileLogger"/> logger adding the appenders to
    /// the root repository.
    /// </summary>
    /// <remarks></remarks>
    public void Configure() {
      // create a new logger into the repository of the current assembly.
      ILoggerRepository root_repository =
        LogManager.GetRepository(Assembly.GetExecutingAssembly());

      Logger nohros_file_logger =
        root_repository.GetLogger("NohrosFileAppender") as Logger;

      // create the layout and appender for log messages
      PatternLayout layout = new PatternLayout();
      layout.ConversionPattern = layout_pattern_;
      layout.ActivateOptions();

      FileAppender appender = new FileAppender();
      appender.Name = "NohrosCommonFileAppender";
      appender.File = log_file_path_;
      appender.AppendToFile = true;
      appender.Layout = layout;
      appender.Threshold = Level.All;
      appender.ActivateOptions();

      // add the appender to the root repository
      nohros_file_logger.Parent.AddAppender(appender);

      root_repository.Configured = true;

      logger = LogManager.GetLogger("NohrosFileLogger");
    }
  }
}
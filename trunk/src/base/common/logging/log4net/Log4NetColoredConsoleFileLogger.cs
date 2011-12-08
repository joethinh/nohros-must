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
  /// A generic logger that uses the third party log4net logging library and
  /// logs messages to the system console.
  /// </summary>
  /// <remarks>
  /// <see cref="Log4NetColoredConsoleLogger"/> appends log events to the
  /// standard output stream ot the error output stream using layout defined
  /// by the user. It also allows the color of a specific type of message to
  /// be set.
  /// <para>
  /// The default threshold level is INFO and could be overloaded on the
  /// nohros configuration file.
  /// </para>
  /// </remarks>
  public class Log4NetColoredConsoleFileLogger: Log4NetLogger
  {
    string layout_pattern_;
    string log_file_path_;
    ColoredConsoleAppender.LevelColors level_colors_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the ConsoleLogger class.
    /// </summary>
    public Log4NetColoredConsoleFileLogger(string layout_pattern,
      string log_file_path) {

      layout_pattern_ = layout_pattern;
      log_file_path_ = log_file_path;

      level_colors_ = new ColoredConsoleAppender.LevelColors();
      level_colors_.Level = Level.Error;
      level_colors_.ForeColor = ColoredConsoleAppender.Colors.Red;
    }
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
      layout.ConversionPattern = layout_pattern_;
      layout.ActivateOptions();

      // create the console appender
      ColoredConsoleAppender console_appender = new ColoredConsoleAppender();
      console_appender.Name = "NohrosCommonConsoleAppender";
      console_appender.Layout = layout;
      console_appender.Target = "Console.Out";
      console_appender.Threshold = Level.All;
      console_appender.ActivateOptions();

      level_colors_.ActivateOptions();
      // create the default colot mapping or add the user supplied
      // to the appender.
      console_appender.AddMapping(level_colors_);

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

    /// <summary>
    /// Gets the mappings between the level that a logging call is made at
    /// and the color it should be displayed as.
    /// </summary>
    /// <remarks>
    /// The default level color used the default console color for all
    /// levels except for ERROR that uses the red color as foreground color.
    /// </remarks>
    public ColoredConsoleAppender.LevelColors LevelColors {
      get { return level_colors_; }
    }
  }
}
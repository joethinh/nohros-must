using System;
using System.Reflection;

using log4net;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace Nohros.Logging.log4net
{
  /// <summary>
  /// A generic logger that uses the third party log4net logging library and
  /// logs messages to the system console.
  /// </summary>
  /// <remarks>
  /// <see cref="ColoredConsoleLogger"/> appends log events to the
  /// standard output stream ot the error output stream using layout defined
  /// by the user. It also allows the color of a specific type of message to
  /// be set.
  /// <para>
  /// The default threshold level is INFO and could be overloaded on the
  /// nohros configuration file.
  /// </para>
  /// </remarks>
  public class ColoredConsoleLogger: AbstractLogger
  {
    string layout_pattern_;
    ColoredConsoleAppender.LevelColors[] level_colors_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the ConsoleLogger class.
    /// </summary>
    public ColoredConsoleLogger(string layout_pattern) {
      layout_pattern_ = layout_pattern;
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

      // create the appender
      ColoredConsoleAppender appender = new ColoredConsoleAppender();
      appender.Name = "NohrosCommonConsoleAppender";
      appender.Layout = layout;
      appender.Target = "Console.Out";
      appender.Threshold = Level.All;

      if (level_colors_ == null) {
        level_colors_ = GetDefaultLevelsColors();
      }

      for (int i = 0, j = level_colors_.Length; i < j; i++) {
        // activate the level colors options and add it to the appender.
        ColoredConsoleAppender.LevelColors level_colors = level_colors_[i];
        if (level_colors != null) {
          level_colors.ActivateOptions();
          appender.AddMapping(level_colors);
        }
      }

      appender.ActivateOptions();

      nohros_console_logger.Parent.AddAppender(appender);

      root_repository.Configured = true;

      logger = LogManager.GetLogger("NohrosConsoleLogger");
    }

    ColoredConsoleAppender.LevelColors[] GetDefaultLevelsColors() {
      // Define the default color mapping for the levels that have lower
      // priority than the the ERROR log. The level color mapping that will
      // be used to log a message will be the nearest mapping value for the
      // level that is equal to os less than the level of the message. So, in
      // order to log all messages to a specific color scheme we only need to
      // set the level of the lowest level in logger level hierarchy, that is
      // the lower that the level that is explicit specified, that is the
      // level of [ERROR].
      Level[] levels = new Level[] {
        Level.Warn, Level.Notice, Level.Info, Level.Debug, Level.Fine,
        Level.Trace, Level.Finer, Level.Verbose, Level.Finest, Level.All
      };

      int levels_length = levels.Length;
      ColoredConsoleAppender.LevelColors[] levels_colors =
        new ColoredConsoleAppender.LevelColors[levels_length + 1];

      for (int i = 0; i < levels_length; i++) {
        ColoredConsoleAppender.LevelColors level_colors =
          new ColoredConsoleAppender.LevelColors();

        level_colors.ForeColor = ColoredConsoleAppender.Colors.Green;
        level_colors.Level = levels[i];
        levels_colors[i] = level_colors;
      }

      // define the default color mapping for the ERROR level.
      levels_colors[levels_length] = new ColoredConsoleAppender.LevelColors();
      levels_colors[levels_length].Level = Level.Error;
      levels_colors[levels_length].ForeColor =
        ColoredConsoleAppender.Colors.Red;

      return levels_colors;
    }

    /// <summary>
    /// Gets the mappings between the level that a logging call is made at
    /// and the color it should be displayed as.
    /// </summary>
    /// <remarks>
    /// The default level color used the default console color for all
    /// levels except for ERROR that uses the red color as foreground color.
    /// </remarks>
    public ColoredConsoleAppender.LevelColors[] LevelColors {
      get { return level_colors_; }
      set {
        if (value == null)
          throw new ArgumentNullException("value");
      }
    }
  }
}
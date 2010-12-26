using System;
using System.Collections.Generic;
using System.Text;

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
    /// This is a generic logger that loads automatically and configures itself through the code. The messages
    /// are logged to the console window.
    /// <para>
    /// The pattern used to log message are:
    ///     . "[%date %-5level/%thread] %message%newline %exception" for the non-debug messages.
    /// </para>
    /// <para>
    /// The default threshold level is INFO and could be overloaded on the nohros configuration file.
    /// </para>
    /// </remarks>
    public class ConsoleLogger
    {
        const string kLogMessagePattern = "[%date %-5level/%thread] %message%newline %exception";

        ILog logger_;
        static ConsoleLogger current_process_logger_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the ConsoleLogger class.
        /// </summary>
        public ConsoleLogger() { }

        /// <summary>
        /// Initializes the singleton process's logger instance.
        /// </summary>
        static ConsoleLogger() {
            current_process_logger_ = new ConsoleLogger();
            current_process_logger_.Configure();
        }
        #endregion

        /// <summary>
        /// Configures the <see cref="FileLogger"/> logger adding the appenders to the root repository.
        /// </summary>
        public void Configure() {
            // create the layout
            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = kLogMessagePattern;
            layout.ActivateOptions();

            // create the appender
            ConsoleAppender appender = new ConsoleAppender();
            appender.Name = "NohrosConsoleAppender";
            appender.Layout = layout;
            appender.Target = "Console.Out";
            appender.Threshold = Level.Info;
            appender.ActivateOptions();

            Logger root = ((Hierarchy)LogManager.GetRepository()).Root;
            root.AddAppender(appender);
            root.Repository.Configured = true;

            logger_ = LogManager.GetLogger(root.Name);
        }

        /// <summary>
        /// Gets the current process logger.
        /// </summary>
        public static ConsoleLogger ForCurrentProcess {
            get { return current_process_logger_; }
        }

        /// <summary>
        /// Gets or sets the threshold level of the logger repository.
        /// </summary>
        internal Level Threshold {
            get { return logger_.Logger.Repository.Threshold; }
            set { logger_.Logger.Repository.Threshold = value; }
        }

        /// <summary>
        /// Gets the <see cref="ILog"/> object used by application to log messages.
        /// </summary>
        /// <remarks>
        /// Attempt to get the value of this property returns null if the logger was not previously configured.
        /// </remarks>
        public ILog Logger {
            get { return logger_; }
        }
    }
}

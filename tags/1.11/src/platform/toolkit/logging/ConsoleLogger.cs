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

namespace Nohros.Toolkit.Logging
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
    ///     . "[%date %-5level/%thread] %line %message%newline %exception" for debug messages.
    /// </para>
    /// </remarks>
    public class ConsoleLogger
    {
        const string kReleasePattern = "[%date %-5level/%thread] %message%newline %exception";
        const string kDebugPattern = "[%date %-5level/%thread] %line %message%newline %exception";

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
            // configure the release logger
            ConsoleAppender release_appender = new ConsoleAppender();
            release_appender.Name = "ReleaseLogger";
            release_appender.Target = "Console.Out";
            release_appender.Layout = new PatternLayout(kReleasePattern);
            release_appender.Threshold = Level.Info;

            // configure the debug logger
            ConsoleAppender debug_appender = new ConsoleAppender();
            debug_appender.Name = "DebugAppender";
            release_appender.Target = "Console.Out";
            debug_appender.Layout = new PatternLayout(kDebugPattern);
            debug_appender.Threshold = Level.Info;

            // append the loggers the the root and instantiate it.
            Logger root = ((Hierarchy)LogManager.GetRepository()).Root;
            root.AddAppender(release_appender);
            root.Repository.Configured = true;

            logger_ = LogManager.GetLogger(typeof(FileLogger));
        }

        /// <summary>
        /// Gets the current process logger.
        /// </summary>
        public static ConsoleLogger ForCurrentProcess {
            get { return current_process_logger_; }
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

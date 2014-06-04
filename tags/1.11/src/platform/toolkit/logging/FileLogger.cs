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
    /// are logged to a file that resides on the same folder of the caller application base directory.The name of
    /// the file is nohros-logger.log for non-debug messages and nohros-logger.debug for debug messages.
    /// <para>
    /// The pattern used to log message are:
    ///     . "[%date %-5level/%thread] %message%newline %exception" for non-debug messages.
    ///     . "[%date %-5level/%thread] %line %message%newline %exception" for debug messages.
    /// </para>
    /// </remarks>
    public class FileLogger
    {
        const string kReleasePattern = "[%date %-5level/%thread] %message%newline %exception";
        const string kDebugPattern = "[%date %-5level/%thread] %line %message%newline %exception";
        const string kNonDebugFileName = "nohros-logger.log";
        const string kDebugFileName = "nohros-logger.debug";

        ILog logger_;
        static FileLogger current_process_logger_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the Logger class.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        /// <remarks>
        /// The logger is not configured here you need to call the <see cref="Configure"/> method to
        /// configure the logger.
        /// </remarks>
        public FileLogger() { }

        /// <summary>
        /// Initializes the singleton process's logger instance.
        /// </summary>
        static FileLogger() {
            current_process_logger_ = new FileLogger();
            current_process_logger_.Configure();
        }
        #endregion

        /// <summary>
        /// Configures the <see cref="FileLogger"/> logger adding the appenders to the root repository.
        /// </summary>
        public void Configure() {
            // configure the release logger
            FileAppender release_appender = new FileAppender();
            release_appender.Name = "ReleaseLogger";
            release_appender.File = kNonDebugFileName;
            release_appender.AppendToFile = true;
            release_appender.LockingModel = new FileAppender.MinimalLock();
            release_appender.Layout = new PatternLayout(kReleasePattern);
            release_appender.Threshold = Level.Info;

            // configure the debug logger
            FileAppender debug_appender = new FileAppender();
            debug_appender.Name = "DebugAppender";
            debug_appender.File = kDebugFileName;
            debug_appender.LockingModel = new FileAppender.MinimalLock();
            debug_appender.Layout = new PatternLayout(kDebugPattern);
            debug_appender.Threshold = Level.Info;

            // append the loggers the the root and instantiate it.
            Logger root =((Hierarchy)LogManager.GetRepository()).Root;
            root.AddAppender(release_appender);
            root.Repository.Configured = true;

            logger_ = LogManager.GetLogger(typeof(FileLogger));
        }

        /// <summary>
        /// Gets the current process logger.
        /// </summary>
        public static FileLogger ForCurrentProcess {
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
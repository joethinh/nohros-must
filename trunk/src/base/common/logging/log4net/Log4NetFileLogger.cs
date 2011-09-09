using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
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
    /// This is a generic logger that loads automatically and configures itself through the code. The messages
    /// are logged to a file that resides on the same folder of the caller application base directory.The name of
    /// the file is nohros-logger.log.
    /// <para>
    /// The pattern used to log message are:
    ///     . "[%date %-5level/%thread] %message%newline %exception".
    /// </para>
    /// <para>
    /// The default threshold level is INFO and could be overloaded on the nohros configuration file.
    /// </para>
    /// </remarks>
    internal class Log4NetFileLogger : Log4NetLogger
    {
        const string kLogErrorMessagePattern = "[%date %-5level/%thread] %message%newline %exception";
        const string kLogCommonMessagePattern = "[%date %-5level/%thread] %message%newline";
        const string kFileName = "nohros-logger.log";

        static Log4NetFileLogger current_process_logger_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the Logger class.
        /// </summary>
        /// <remarks>
        /// The logger is not configured here you need to call the <see cref="Configure"/> method to
        /// configure the logger.
        /// </remarks>
        public Log4NetFileLogger() { }

        /// <summary>
        /// Initializes the singleton process's logger instance.
        /// </summary>
        static Log4NetFileLogger() {
            current_process_logger_ = new Log4NetFileLogger();
            current_process_logger_.Configure();
        }
        #endregion

        /// <summary>
        /// Configures the <see cref="FileLogger"/> logger adding the appenders to the root repository.
        /// </summary>
        /// <remarks></remarks>
        public void Configure() {
            // create a new logger into the repository of the current assembly.
            ILoggerRepository root_repository = LogManager.GetRepository(Assembly.GetExecutingAssembly());
            Logger nohros_file_logger = root_repository.GetLogger("NohrosFileAppender") as Logger;

            // create the layout
            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = kLogErrorMessagePattern;
            layout.ActivateOptions();

            // create the appender for error messages
            FileAppender error_appender = new FileAppender();
            error_appender.Name = "NohrosErrorFileAppender";
            error_appender.File = kFileName;
            error_appender.AppendToFile = true;
            error_appender.LockingModel = new FileAppender.MinimalLock();
            error_appender.Layout = layout;
            error_appender.Threshold = Level.Error;
            error_appender.ActivateOptions();

            // create the layout and appender for non error messages
            PatternLayout common_layout = new PatternLayout();
            layout.ConversionPattern = kLogCommonMessagePattern;
            layout.ActivateOptions();

            FileAppender common_appender = new FileAppender();
            common_appender.Name = "NohrosCommonFileAppender";
            common_appender.File = kFileName;
            common_appender.AppendToFile = true;
            common_appender.LockingModel = new FileAppender.MinimalLock();
            common_appender.Layout = common_layout;
            common_appender.Threshold = Level.Info;
            common_appender.ActivateOptions();

            nohros_file_logger.AddAppender(error_appender);
            nohros_file_logger.AddAppender(common_appender);

            logger_ = LogManager.GetLogger("NohrosFileLogger");
        }

        /// <summary>
        /// Gets the current process logger.
        /// </summary>
        public static Log4NetFileLogger ForCurrentProcess {
            get { return current_process_logger_; }
        }

        /// <summary>
        /// Gets or sets the threshold level of the logger repository.
        /// </summary>
        internal LogLevel LogLevel {
            get {
                Level level = logger_.Logger.Repository.Threshold;
                if (level == Level.All) {
                    return LogLevel.All;
                } else if (level == Level.Debug) {
                    return LogLevel.Debug;
                } else if (level == Level.Error) {
                    return LogLevel.Error;
                } else if (level == Level.Fatal) {
                    return LogLevel.Fatal;
                } else if (level == Level.Info) {
                    return LogLevel.Info;
                } else if (level == Level.Off) {
                    return LogLevel.Off;
                } else if (level == Level.Warn) {
                    return LogLevel.Warn;
                } else {
                    return LogLevel.Off;
                }
            }
            set {
                ILoggerRepository repository = logger_.Logger.Repository;
                switch (value) {
                    case LogLevel.All:
                        repository.Threshold = Level.All;
                        break;

                    case LogLevel.Debug:
                        repository.Threshold = Level.Debug;
                        break;

                    case LogLevel.Error:
                        repository.Threshold = Level.Error;
                        break;

                    case LogLevel.Fatal:
                        repository.Threshold = Level.Fatal;
                        break;

                    case LogLevel.Info:
                        repository.Threshold = Level.Info;
                        break;

                    case LogLevel.Off:
                        repository.Threshold = Level.Off;
                        break;

                    case LogLevel.Trace:
                        repository.Threshold = Level.Trace;
                        break;

                    case LogLevel.Warn:
                        repository.Threshold = Level.Warn;
                        break;
                }
            }
        }
    }
}
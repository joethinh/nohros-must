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
    public class FileLogger
    {
        const string kLogMessagePattern = "[%date %-5level/%thread] %message%newline %exception";
        const string kFileName = "nohros-logger.log";

        ILog logger_;
        static FileLogger current_process_logger_;
        string logger_file_path_;

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
        /// <remarks></remarks>
        public void Configure() {
            // create the layout
            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = kLogMessagePattern;
            layout.ActivateOptions();

            // create the appender
            FileAppender appender = new FileAppender();
            appender.Name = "NohrosFileAppender";
            appender.File = kFileName;
            appender.AppendToFile = true;
            appender.LockingModel = new FileAppender.MinimalLock();
            appender.Layout = layout;
            appender.Threshold = Level.Info;
            appender.ActivateOptions();

            logger_file_path_ = appender.File;

            Logger root = ((Hierarchy)LogManager.GetRepository()).Root;
            root.AddAppender(appender);
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

        /// <summary>
        /// Gets the fully qualified path of the log file.
        /// </summary>
        public string LoggerFilePath {
            get { return logger_file_path_; }
        }
    }
}
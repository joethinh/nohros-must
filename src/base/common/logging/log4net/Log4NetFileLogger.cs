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

        string log_file_path_;
        
        #region .ctor
        /// <summary>
        /// Initializes a new instance of the Logger class by using the specified string as the path
        /// to the log file.
        /// </summary>
        /// <remarks>
        /// The logger is not functional at this point, you need to call the <see cref="Configure"/>
        /// method to in order to make the logger usable.
        /// </remarks>
        public Log4NetFileLogger(string log_file_path) {
#if DEBUG
            if (log_file_path == null || log_file_path.Length == 0) {
                throw new ArgumentException("log_file_path is null or empty");
            }
#endif
            log_file_path_ = log_file_path;
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
            error_appender.File = log_file_path_;
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
            common_appender.File = log_file_path_;
            common_appender.AppendToFile = true;
            common_appender.LockingModel = new FileAppender.MinimalLock();
            common_appender.Layout = common_layout;
            common_appender.Threshold = Level.Info;
            common_appender.ActivateOptions();

            nohros_file_logger.AddAppender(error_appender);
            nohros_file_logger.AddAppender(common_appender);

            logger_ = LogManager.GetLogger("NohrosFileLogger");
        }
    }
}
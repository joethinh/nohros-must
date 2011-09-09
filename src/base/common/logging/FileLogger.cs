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
    /// The default threshold level is INFO and could be overloaded on the configuration file.
    /// </para>
    /// </remarks>
    public class FileLogger
    {
        const string kFileName = "nohros-logger.log";

        static ILogger current_process_logger_;

        #region .ctor
        /// <summary>
        /// Initializes the singleton process's logger instance.
        /// </summary>
        static FileLogger() {
            Log4NetFileLogger logger = new Log4NetFileLogger(kFileName);
            logger.Configure();

            current_process_logger_ = logger as ILogger;
        }
        #endregion

        /// <summary>
        /// Gets the current process logger.
        /// </summary>
        public static ILogger ForCurrentProcess {
            get { return (ILogger)current_process_logger_; }
        }
    }
}
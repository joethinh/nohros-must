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
    public class FileLogger
    {
        const string kReleasePattern = "[%date %-5level/%thread] %message%newline %exception";
        const string kDebugPattern = "[%date %-5level/%thread] %line %message%newline %exception";

        ILog logger_;
        static FileLogger current_process_logger_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the Logger class by using the specified logger name.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        public FileLogger() { }

        /// <summary>
        /// Initializes the singleton process's logger instance.
        /// </summary>
        static FileLogger() {
            current_process_logger_ = new FileLogger();
            current_process_logger_.Configure();
        }
        #endregion

        public void Configure() {
            // configure the release logger
            FileAppender release_appender = new FileAppender();
            release_appender.Name = "ReleaseLogger";
            release_appender.File = "nohros.logger";
            release_appender.AppendToFile = true;
            release_appender.LockingModel = new FileAppender.MinimalLock();
            release_appender.Layout = new PatternLayout(kReleasePattern);
            release_appender.Threshold = Level.Info;

            // configure the debug logger
            FileAppender debug_appender = new FileAppender();
            debug_appender.Name = "DebugAppender";
            debug_appender.File = "nohros.logger";
            debug_appender.LockingModel = new FileAppender.MinimalLock();
            debug_appender.Layout = new PatternLayout(kDebugPattern);
            debug_appender.Threshold = Level.Info;

            // append the loggers the the root and instantiate it.
            Logger root =((Hierarchy)LogManager.GetRepository()).Root;
            root.AddAppender(release_appender);
            root.Repository.Configured = true;

            logger_ = LogManager.GetLogger(typeof(FileLogger));
        }

        public static FileLogger ForCurrentProcess {
            get { return current_process_logger_; }
        }

        public ILog Logger {
            get { return logger_; }
        }
    }
}
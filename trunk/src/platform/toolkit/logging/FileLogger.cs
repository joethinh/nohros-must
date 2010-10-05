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
        const string kLoggerPattern = "[%date %-5level/%thread] %message%newline %exception";

        string name_;
        ILog logger_;

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the Logger class by using the specified logger name.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        public FileLogger() { }
        #endregion

        public void Configure() {

            // configure the release logger
            FileAppender release_appender = new FileAppender();
            release_appender.Name = "ReleaseLogger";
            release_appender.File = "nohros.logger";
            release_appender.AppendToFile = true;
            release_appender.LockingModel = new FileAppender.MinimalLock();
            release_appender.Layout = new PatternLayout(kLoggerPattern);
            release_appender.Threshold = Level.Info;

            Logger root =((Hierarchy)LogManager.GetRepository()).Root;
            root.AddAppender(release_appender);
            root.Repository.Configured = true;

            logger_ = LogManager.GetLogger(typeof(FileLogger));
        }

        public ILog Logger {
            get { return logger_; }
        }
    }
}

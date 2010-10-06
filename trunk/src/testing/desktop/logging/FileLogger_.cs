using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using log4net;
using Nohros.Logging;

namespace Nohros.Test.Logging
{
    [TestFixture]
    public class FileLogger_
    {
        [Test]
        public void LoggerWithoutConfigure() {
            FileLogger logger = new FileLogger();
            Assert.IsNull(logger.Logger);
        }

        [Test]
        public void Configure() {
            FileLogger logger = new FileLogger();
            logger.Configure();
            Assert.IsNotNull(logger.Logger);
        }

        [Test]
        public void ForCurrentProcess() {
            FileLogger logger = FileLogger.ForCurrentProcess;
            Assert.IsNotNull(logger);
        }
    }
}

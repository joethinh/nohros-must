using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Logging.Tests
{
  public static class Program
  {
    public static void Main() {
      Log4NetFileLogger logger = new Log4NetFileLogger("nohros.logger");
      logger.Configure();

      logger.Info("logging-message");
    }
  }
}

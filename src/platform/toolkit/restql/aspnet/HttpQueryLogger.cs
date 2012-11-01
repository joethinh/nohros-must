using System;
using Nohros.Logging;

namespace Nohros.RestQL
{
  public class HttpQueryLogger : ForwardingLogger
  {
    static readonly HttpQueryLogger current_process_logger_;

    #region .ctor
    static HttpQueryLogger() {
      current_process_logger_ = new HttpQueryLogger(new NOPLogger());
    }
    #endregion

    #region .ctor
    public HttpQueryLogger(ILogger logger) : base(logger) {
    }
    #endregion

    public static HttpQueryLogger ForCurrentProcess {
      get { return current_process_logger_; }
    }
  }
}

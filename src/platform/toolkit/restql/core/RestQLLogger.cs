using System;
using Nohros.Logging;

namespace Nohros.Toolkit.RestQL
{
  /// <summary>
  /// A implementation of the <see cref="IRestQLLogger"/> that uses the
  /// nohros must framework.
  /// </summary>
  /// <remarks>
  /// This class uses the nohros must framework and is the only point where
  /// this dependency exists. Clients should call the
  /// <see cref="ForCurrentProcess"/> method to obtain an instance of the
  /// <see cref="IRestQLLogger"/> class, and uses it to log messages.
  /// <para>
  /// By default the <see cref="NOPLogger"/> is returned by the
  /// <see cref="ForCurrentProcess"/> method. The application must configure
  /// the correct logger on the app initialization.
  /// </para>
  /// </remarks>
  public class RestQLLogger : ForwardingLogger
  {
    static readonly RestQLLogger current_process_logger_;

    #region .ctor
    static RestQLLogger() {
      current_process_logger_ = new RestQLLogger(new NOPLogger());
    }
    #endregion

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="RestQLLogger"/>
    /// class by using the specified <see cref="ILogger"/> interface.
    /// </summary>
    public RestQLLogger(ILogger logger) : base(logger) {
    }
    #endregion

    /// <summary>
    /// Gets the current process logger.
    /// </summary>
    public static RestQLLogger ForCurrentProcess {
      get { return current_process_logger_; }
    }
  }
}

using System;

namespace Nohros.Logging
{
  /// <summary>
  /// A implementation of the <see cref="ILogger"/> used for internal library
  /// logging.
  /// </summary>
  /// <remarks>
  /// This class uses the nohros must framework and is the only point where
  /// this dependency exists. Clients should call the
  /// <see cref="ForCurrentProcess"/> method to obtain an instance of the
  /// <see cref="ILogger"/> class, and uses it to log messages.
  /// <para>
  /// By default the <see cref="NOPLogger"/> is returned by the
  /// <see cref="ForCurrentProcess"/> method. The client should configure
  /// the correct logger on the app initialization.
  /// </para>
  /// </remarks>
  public class MustLogger: ILogger
  {
    readonly static ILogger current_process_logger_;

    readonly ILogger internal_logger_;

    #region .ctor
    /// <summary>
    /// Initializes the singleton process's logger instance.
    /// </summary>
    static MustLogger() {
      // set the logger to be used, configure it and
      // do some initialization stuffs
      //
      // ex.
      //   FileLogger internal_logger =
      //     FileLogger.ForCurrentProcess();

      // creates a instance of the NOPLogger to ensure that
      // the ForCurrentProcess always returns a valid logger.
      // Note that the client is responsible to set the value
      // of the singleton logger object.
      NOPLogger internal_logger = new NOPLogger();

      // initialize a new instance of the MustLogger using the
      // previously instantiated logger.
      MustLogger logger = new MustLogger(internal_logger);

      current_process_logger_ = logger as IMustLogger;
    }
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="MustLogger"/>
    /// class by using the specified <see cref="ILogger"/> interface.
    /// </summary>
    public MustLogger(ILogger logger) {
      internal_logger_ = logger;
    }

    /// <summary>
    /// Gets the current process logger.
    /// </summary>
    public static ILogger ForCurrentProcess {
      get {
        return current_process_logger_;
      }
    }

    #region IsEnabled

    /// <inherit />
    public bool IsDebugEnabled {
      get {
        return internal_logger_.IsDebugEnabled;
      }
    }

    /// <inherit />
    public bool IsErrorEnabled {
      get {
        return internal_logger_.IsErrorEnabled;
      }
    }

    /// <inherit />
    public bool IsFatalEnabled {
      get {
        return internal_logger_.IsFatalEnabled;
      }
    }

    /// <inherit />
    public bool IsInfoEnabled {
      get {
        return internal_logger_.IsInfoEnabled;
      }
    }

    /// <inherit />
    public bool IsWarnEnabled {
      get {
        return internal_logger_.IsWarnEnabled;
      }
    }

    /// <inherit />
    public bool IsTraceEnabled {
      get {
        return internal_logger_.IsTraceEnabled;
      }
    }

    #endregion

    /// <inherit />
    public void Debug(string message) {
      internal_logger_.Debug(message);
    }

    /// <inherit />
    public void Debug(string message, Exception exception) {
      internal_logger_.Debug(message, exception);
    }

    /// <inherit />
    public void Error(string message) {
      internal_logger_.Error(message);
    }

    /// <inherit />
    public void Error(string message, Exception exception) {
      internal_logger_.Error(message, exception);
    }

    /// <inherit />
    public void Fatal(string message) {
      internal_logger_.Fatal(message);
    }

    /// <inherit />
    public void Fatal(string message, Exception exception) {
      internal_logger_.Fatal(message, exception);
    }

    /// <inherit />
    public void Info(string message) {
      internal_logger_.Info(message);
    }

    /// <inherit />
    public void Info(string message, Exception exception) {
      internal_logger_.Info(message, exception);
    }

    /// <inherit />
    public void Warn(string message) {
      internal_logger_.Warn(message);
    }

    /// <inherit />
    public void Warn(string message, Exception exception) {
      internal_logger_.Warn(message, exception);
    }
  }
}
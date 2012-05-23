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
  public class RestQLLogger : IRestQLLogger
  {
    readonly static IRestQLLogger current_process_logger_;
    readonly ILogger internal_logger_;

    #region .ctor
    /// <summary>
    /// Initializes the singleton process's logger instance.
    /// </summary>
    static RestQLLogger() {
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

      // initialize a new instance of the RestQLLogger using the
      // previously instantiated logger.
      RestQLLogger logger = new RestQLLogger(internal_logger);

      current_process_logger_ = logger;
    }
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="RestQLLogger"/>
    /// class by using the specified <see cref="ILogger"/> interface.
    /// </summary>
    public RestQLLogger(ILogger logger) {
      internal_logger_ = logger;
    }

    /// <summary>
    /// Gets the current process logger.
    /// </summary>
    public static IRestQLLogger ForCurrentProcess {
      get {
        return current_process_logger_;
      }
    }

    #region IsEnabled

    /// <inheritdoc />
    public bool IsDebugEnabled {
      get {
        return internal_logger_.IsDebugEnabled;
      }
    }

    /// <inheritdoc />
    public bool IsErrorEnabled {
      get {
        return internal_logger_.IsErrorEnabled;
      }
    }

    /// <inheritdoc />
    public bool IsFatalEnabled {
      get {
        return internal_logger_.IsFatalEnabled;
      }
    }

    /// <inheritdoc />
    public bool IsInfoEnabled {
      get {
        return internal_logger_.IsInfoEnabled;
      }
    }

    /// <inheritdoc />
    public bool IsWarnEnabled {
      get {
        return internal_logger_.IsWarnEnabled;
      }
    }

    /// <inheritdoc />
    public bool IsTraceEnabled {
      get {
        return internal_logger_.IsTraceEnabled;
      }
    }

    #endregion

    /// <inheritdoc />
    public void Debug(string message) {
      internal_logger_.Debug(message);
    }

    /// <inheritdoc />
    public void Debug(string message, Exception exception) {
      internal_logger_.Debug(message, exception);
    }

    /// <inheritdoc />
    public void Error(string message) {
      internal_logger_.Error(message);
    }

    /// <inheritdoc />
    public void Error(string message, Exception exception) {
      internal_logger_.Error(message, exception);
    }

    /// <inheritdoc />
    public void Fatal(string message) {
      internal_logger_.Fatal(message);
    }

    /// <inheritdoc />
    public void Fatal(string message, Exception exception) {
      internal_logger_.Fatal(message, exception);
    }

    /// <inheritdoc />
    public void Info(string message) {
      internal_logger_.Info(message);
    }

    /// <inheritdoc />
    public void Info(string message, Exception exception) {
      internal_logger_.Info(message, exception);
    }

    /// <inheritdoc />
    public void Warn(string message) {
      internal_logger_.Warn(message);
    }

    /// <inheritdoc />
    public void Warn(string message, Exception exception) {
      internal_logger_.Warn(message, exception);
    }
  }
}
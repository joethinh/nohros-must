using System;
using System.Collections.Generic;
using System.Text;

using Nohros.Logging;

namespace Nohros.logging
{
  /// <summary>
  /// A implementation of the <see cref="IChainedLogger"/> that uses the
  /// nohros must framework.
  /// </summary>
  /// <remarks>
  /// This class uses the nohros must framework and is the only point where
  /// this dependency exists. Clients should call the
  /// <see cref="ForCurrentProcess"/> method to obtain an instance of the
  /// <see cref="IChainedLogger"/> class, and uses it to log messages.
  /// <para>
  /// By default the <see cref="NOPLogger"/> is returned by the
  /// <see cref="ForCurrentProcess"/> method. The application must configure
  /// the correct logger on the app initialization.
  /// </para>
  /// </remarks>
  public class ChainedLogger : ILogger
  {
    class LoggerListNode {
      public ILogger logger;
      public LoggerListNode next;

      public LoggerListNode(ILogger logger) {
        this.logger = logger;
        next = null;
      }
    }

    class LoggerList {
      public LoggerListNode head;

      public LoggerList(ILogger logger) {
        head = new LoggerListNode(logger);
      }
    }

    delegate void LogDelegate(string message, ILogger logger);
    delegate void LogWithExceptionDelegate(string message, ILogger logger, Exception exeption);

    readonly LoggerList logger_chain_;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChainedLogger"/> class
    /// by using the specified logger.
    /// </summary>
    /// <param name="logger">The first logger of the chain.</param>
    /// <remarks>
    /// The first logger of the chain will dictate the behavior of the methods
    /// used to check if a particular level is enabled or not. Note that the
    /// first logger dictates onle the level of the <see cref="ChainedLogger"/>
    /// class, each logger uses it own level to perform logging
    /// operations.
    /// </remarks>
    public ChainedLogger(ILogger logger) {
      logger_chain_ = new LoggerList(logger);
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

    void Log(string message, LogDelegate functor) {
      LoggerListNode current = logger_chain_.head;
      while(current.next != null) {
        functor(message, current.logger);
        current = current.next;
      }
    }

    void LogWithException(string message, Exception exception, LogWithExceptionDelegate functor) {
      LoggerListNode current = logger_chain_.head;
      while (current != null) {
        functor(message, current.logger, exception);
        current = current.next;
      }
    }

    /// <inherit />
    public void Debug(string message) {
      Log(message,
        delegate(string msg, ILogger logger) {
          logger.Debug(msg);
        });
    }

    /// <inherit />
    public void Debug(string message, Exception exception) {
      LogWithException(message, exception,
        delegate(string msg, ILogger logger, Exception e)
        {
          logger.Debug(msg, e);
        });
    }

    /// <inherit />
    public void Error(string message) {
      Log(message,
        delegate(string msg, ILogger logger)
        {
          logger.Error(msg);
        });
    }

    /// <inherit />
    public void Error(string message, Exception exception) {
      LogWithException(message, exception,
        delegate(string msg, ILogger logger, Exception e)
        {
          logger.Debug(msg, e);
        });
    }

    /// <inherit />
    public void Fatal(string message) {
      Log(message,
        delegate(string msg, ILogger logger)
        {
          logger.Fatal(msg);
        });
    }

    /// <inherit />
    public void Fatal(string message, Exception exception) {
      LogWithException(message, exception,
        delegate(string msg, ILogger logger, Exception e)
        {
          logger.Debug(msg, e);
        });
    }

    /// <inherit />
    public void Info(string message) {
      Log(message,
        delegate(string msg, ILogger logger)
        {
          logger.Info(msg);
        });
    }

    /// <inherit />
    public void Info(string message, Exception exception) {
      LogWithException(message, exception,
        delegate(string msg, ILogger logger, Exception e)
        {
          logger.Debug(msg, e);
        });
    }

    /// <inherit />
    public void Warn(string message) {
      Log(message,
        delegate(string msg, ILogger logger)
        {
          logger.Warn(msg);
        });
    }

    /// <inherit />
    public void Warn(string message, Exception exception) {
      LogWithException(message, exception,
        delegate(string msg, ILogger logger, Exception e)
        {
          logger.Debug(msg, e);
        });
    }
  }
}
using System;

namespace Nohros.Logging
{
  /// <summary>
  /// An implementation of the <see cref="ILogger"/> which forwards all
  /// its methods to another <see cref="ILogger"/> object.
  /// </summary>
  public class ForwardingLogger: IForwardingLogger
  {
    /// <summary>
    /// The backing logger.
    /// </summary>
    protected ILogger logger;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ForwardingLogger"/> using
    /// the specified <see cref="ILogger"/> as backing logger.
    /// </summary>
    /// <param name="logger">
    /// The logger instance that methods are forwarder to.
    /// </param>
    public ForwardingLogger(ILogger logger) {
      this.logger = logger;
    }
    #endregion

    /// <inheritdoc />
    public bool IsDebugEnabled {
      get { return logger.IsDebugEnabled; }
    }

    /// <inheritdoc />
    public bool IsErrorEnabled {
      get { return logger.IsErrorEnabled; }
    }

    /// <inheritdoc />
    public bool IsFatalEnabled {
      get { return logger.IsFatalEnabled; }
    }

    /// <inheritdoc />
    public bool IsInfoEnabled {
      get { return logger.IsInfoEnabled; }
    }

    /// <inheritdoc />
    public bool IsWarnEnabled {
      get { return logger.IsWarnEnabled; }
    }

    /// <inheritdoc />
    public bool IsTraceEnabled {
      get { return logger.IsTraceEnabled; }
    }

    /// <inheritdoc />
    public void Debug(string message) {
      logger.Debug(message);
    }

    /// <inheritdoc />
    public void Debug(string message, Exception exception) {
      logger.Debug(message, exception);
    }

    /// <inheritdoc />
    public void Error(string message) {
      logger.Error(message);
    }

    /// <inheritdoc />
    public void Error(string message, Exception exception) {
      logger.Error(message, exception);
    }

    /// <inheritdoc />
    public void Fatal(string message) {
      logger.Fatal(message);
    }

    /// <inheritdoc />
    public void Fatal(string message, Exception exception) {
      logger.Fatal(message, exception);
    }

    /// <inheritdoc />
    public void Info(string message) {
      logger.Info(message);
    }

    /// <inheritdoc />
    public void Info(string message, Exception exception) {
      logger.Info(message, exception);
    }

    /// <inheritdoc />
    public void Warn(string message) {
      logger.Warn(message);
    }

    /// <inheritdoc />
    public void Warn(string message, Exception exception) {
      logger.Warn(message, exception);
    }

    /// <summary>
    /// Gets the backing logger instance that methods are forwarder to.
    /// </summary>
    public virtual ILogger Logger {
      get { return logger; }
      set { logger = value; }
    }
  }
}

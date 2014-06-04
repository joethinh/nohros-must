using System;

namespace Nohros.Logging
{
  /// <summary>
  /// A implementation of the <see cref="ILogger"/> that log all messages
  /// using the attached <see cref="System.Console"/>.
  /// </summary>
  public sealed class ConsoleLogger : ILogger
  {
    readonly LogLevel level_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleLogger"/> class
    /// using the specified <see cref="LogLevel"/>.
    /// </summary>
    /// <param name="level"></param>
    public ConsoleLogger(LogLevel level) {
      level_ = level;
    }
    #endregion

    /// <inheritdoc/>
    public void Debug(string message) {
      Log(LogLevel.Debug, message);
    }

    /// <inheritdoc/>
    public void Debug(string message, Exception exception) {
      Log(LogLevel.Debug, message, exception);
    }

    /// <inheritdoc/>
    public void Error(string message) {
      Log(LogLevel.Error, message);
    }

    /// <inheritdoc/>
    public void Error(string message, Exception exception) {
      Log(LogLevel.Error, message, exception);
    }

    /// <inheritdoc/>
    public void Fatal(string message) {
      Log(LogLevel.Fatal, message);
    }

    /// <inheritdoc/>
    public void Fatal(string message, Exception exception) {
      Log(LogLevel.Fatal, message, exception);
    }

    /// <inheritdoc/>
    public void Info(string message) {
      Log(LogLevel.Info, message);
    }

    /// <inheritdoc/>
    public void Info(string message, Exception exception) {
      Log(LogLevel.Info, message, exception);
    }

    /// <inheritdoc/>
    public void Warn(string message) {
      Log(LogLevel.Warn, message);
    }

    /// <inheritdoc/>
    public void Warn(string message, Exception exception) {
      Log(LogLevel.Warn, message, exception);
    }

    public bool IsDebugEnabled {
      get { return level_ < LogLevel.Debug; }
    }

    public bool IsErrorEnabled {
      get { return level_ < LogLevel.Debug; }
    }

    public bool IsFatalEnabled {
      get { return level_ < LogLevel.Debug; }
    }

    public bool IsInfoEnabled {
      get { return level_ < LogLevel.Debug; }
    }

    public bool IsWarnEnabled {
      get { return level_ < LogLevel.Debug; }
    }

    public bool IsTraceEnabled {
      get { return level_ < LogLevel.Debug; }
    }

    void Log(LogLevel level, string message) {
      if (level_ < level) {
        Console.WriteLine(message);
      }
    }

    void Log(LogLevel level, string message, Exception exception) {
      if (level_ < level) {
        Console.WriteLine(message);
        Console.WriteLine(exception);
      }
    }
  }
}

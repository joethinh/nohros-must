using System;
using System.Collections.Generic;
using System.Text;

namespace Nohros.Logging
{
  /// <summary>
  /// No-operation implementation og <see cref="ILogger"/> interface.
  /// </summary>
  /// <remarks>
  /// Since this logger does not do anything the Is[...] methods always
  /// returns null and the level of the logger will be always Off, even if
  /// the user set it to a diferrent value.
  /// </remarks>
  public class NOPLogger: ILogger
  {
    #region .ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="NOPLogger"/> class.
    /// </summary>
    public NOPLogger() { }
    #endregion

    #region IsEnabled

    /// <inherit />
    public bool IsDebugEnabled { get { return false; } }

    /// <inherit />
    public bool IsErrorEnabled { get { return false; } }

    /// <inherit />
    public bool IsFatalEnabled { get { return false; } }

    /// <inherit />
    public bool IsInfoEnabled { get { return false; } }

    /// <inherit />
    public bool IsWarnEnabled { get { return false; } }

    /// <inherit />
    public bool IsTraceEnabled { get { return false; } }

    #endregion

    /// <inherit />
    public void Debug(string message) { }

    /// <inherit />
    public void Debug(string message, Exception exception) { }

    /// <inherit />
    public void Error(string message) { }

    /// <inherit />
    public void Error(string message, Exception exception) { }

    /// <inherit />
    public void Fatal(string message) { }

    /// <inherit />
    public void Fatal(string message, Exception exception) { }

    /// <inherit />
    public void Info(string message) { }

    /// <inherit />
    public void Info(string message, Exception exception) { }

    /// <inherit />
    public void Warn(string message) { }

    /// <inherit />
    public void Warn(string message, Exception exception) { }

    /// <summary>
    /// Gets or sets the threshold level of the logger repository.
    /// </summary>
    /// <remarks>Always return null. Even if a user set it to a
    /// diferent value.</remarks>
    internal LogLevel LogLevel {
      get { return LogLevel.Off; }
      set { }
    }
  }
}

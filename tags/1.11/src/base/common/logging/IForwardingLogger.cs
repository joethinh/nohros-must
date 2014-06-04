using System;

namespace Nohros.Logging
{
  /// <summary>
  /// An implementation of the <see cref="ILogger"/> interface which forwards
  /// all its methods to another <see cref="ILogger"/> object.
  /// </summary>
  public interface IForwardingLogger : ILogger
  {
    /// <summary>
    /// Gets the backing logger instance that methods are forwarder to.
    /// </summary>
    ILogger Logger { get; set; }
  }
}

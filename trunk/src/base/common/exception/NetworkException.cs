using System;
using System.Runtime.Serialization;

namespace Nohros
{
  /// <summary>
  /// Thrown if there is a network communication failure.
  /// </summary>
  public class NetworkException : ProviderException
  {
    #region .ctor
    /// <summary>
    /// Creates a new instance of the <see cref="NetworkException"/> class.
    /// </summary>
    public NetworkException() {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="NetworkException"/> class.
    /// </summary>
    /// <param source="message">A message describing why this
    /// <see cref="NetworkException"/>was throw</param>
    public NetworkException(string message) : base(message) {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="NetworkException"/> class.
    /// </summary>
    /// <param source="inner_exception">
    /// The exception that caused this <see cref="NetworkException"/> to be
    /// throw.
    /// </param>
    public NetworkException(Exception inner_exception) : base(inner_exception) {
    }

    /// <summary>
    /// Creates a new instance if the <see cref="NetworkException"/> class.
    /// </summary>
    /// <param source="info">The object that holds the information to
    /// deserialize.</param>
    /// <param source="context">Contextual information about the source or
    /// destination.</param>
    protected NetworkException(SerializationInfo info, StreamingContext context)
      : base(info, context) {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="NetworkException"/> class.
    /// </summary>
    /// <param source="message">A message describing why this
    /// <see cref="NetworkException"/>was throw.</param>
    /// <param source="inner_exception">The exception that caused this
    /// <see cref="NetworkException"/>to be throw.</param>
    public NetworkException(string message, Exception inner_exception)
      : base(message, inner_exception) {
    }
    #endregion
  }
}

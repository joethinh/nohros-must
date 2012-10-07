using System;
using System.Runtime.Serialization;

namespace Nohros
{
  /// <summary>
  /// An exception that is throwed when a portion of code that should be not
  /// reached is reached.
  /// </summary>
  public class NotReachedException : Exception
  {
    #region .ctor
    /// <summary>
    /// Creates a new instance_ of the <see cref="NotReachedException"/> class.
    /// </summary>
    public NotReachedException() {
    }

    /// <summary>
    /// Creates a new instance_ of the <see cref="NotReachedException"/> class.
    /// </summary>
    /// <param name="message">A message describing why this
    /// <see cref="NotReachedException"/>was throw</param>
    public NotReachedException(string message) : base(message) {
    }

    /// <summary>
    /// Creates a new instance_ if the <see cref="NotReachedException"/> class.
    /// </summary>
    /// <param name="info">The object that holds the information to
    /// deserialize.</param>
    /// <param name="context">Contextual information about the source or
    /// destination.</param>
    protected NotReachedException(SerializationInfo info,
      StreamingContext context) : base(info, context) {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="NotReachedException"/> class.
    /// </summary>
    /// <param name="message">A message describing why this
    /// <see cref="NotReachedException"/>was throw.</param>
    /// <param name="inner_exception">The exception that caused this
    /// <see cref="NotReachedException"/>to be throw.</param>
    public NotReachedException(string message, Exception inner_exception)
      : base(message, inner_exception) {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="NotReachedException"/> class
    /// using the specified inner exception.
    /// </summary>
    /// <param name="inner_exception">A <see cref="Exception"/> that causes
    /// this exception to be raised.</param>
    public NotReachedException(Exception inner_exception)
      : base(string.Empty, inner_exception) {
    }
    #endregion
  }
}

using System;
using System.Runtime.Serialization;

namespace Nohros
{
  /// <summary>
  /// Exception thrown when attempting to retrieve the result of a task that
  /// aborted by throwing an exception.
  /// </summary>
  public class ExecutionException : Exception
  {
    #region .ctor
    /// <summary>
    /// Creates a new instance_ of the <see cref="ExecutionException"/> class.
    /// </summary>
    public ExecutionException() {
    }

    /// <summary>
    /// Creates a new instance_ of the <see cref="ExecutionException"/> class.
    /// </summary>
    /// <param name="message">A message describing why this
    /// <see cref="ExecutionException"/>was throw</param>
    public ExecutionException(string message) : base(message) {
    }

    /// <summary>
    /// Creates a new instance_ if the <see cref="ExecutionException"/> class.
    /// </summary>
    /// <param name="info">The object that holds the information to
    /// deserialize.</param>
    /// <param name="context">Contextual information about the source or
    /// destination.</param>
    protected ExecutionException(SerializationInfo info,
      StreamingContext context) : base(info, context) {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ExecutionException"/> class.
    /// </summary>
    /// <param name="message">A message describing why this
    /// <see cref="ExecutionException"/>was throw.</param>
    /// <param name="inner_exception">The exception that caused this
    /// <see cref="ExecutionException"/>to be throw.</param>
    public ExecutionException(string message, Exception inner_exception)
      : base(message, inner_exception) {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ExecutionException"/> class
    /// using the specified inner exception.
    /// </summary>
    /// <param name="inner_exception">A <see cref="Exception"/> that causes
    /// this exception to be raised.</param>
    public ExecutionException(Exception inner_exception)
      : base(string.Empty, inner_exception) {
    }
    #endregion
  }
}
